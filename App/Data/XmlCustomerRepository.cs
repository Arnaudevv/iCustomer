// App/Data/XmlCustomerRepository.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using iCustomer.Models;

namespace iCustomer.Data
{
    /// <summary>
    /// Implements ICustomerRepository using an XML file as the backing store.
    ///
    /// DESIGN NOTES
    /// ─────────────
    /// • Monthly expenses are NOT persisted to XML (they are regenerated randomly on load)
    ///   because the task explicitly excludes them from the XML data layer.
    /// • Atomic write strategy: write to a .tmp file first, then rename over the real file.
    ///   If the process crashes during the write the original file is untouched.
    /// • All data is loaded once into an in-memory List (cache) to avoid repeated I/O.
    ///   SaveChanges() flushes the full cache to disk.
    /// </summary>
    public class XmlCustomerRepository : ICustomerRepository
    {
        // ─────────────────────────────────────────────────────────────
        // Configuration
        // ─────────────────────────────────────────────────────────────

        private readonly string _xmlPath;
        private readonly string _tmpPath;

        /// <param name="xmlPath">
        ///   Full path to customers.xml.
        ///   If the file does not exist, an empty store is assumed and the file
        ///   is created on the first call to SaveChanges().
        /// </param>
        public XmlCustomerRepository(string xmlPath)
        {
            _xmlPath = xmlPath ?? throw new ArgumentNullException(nameof(xmlPath));
            _tmpPath = xmlPath + ".tmp";
        }

        // ─────────────────────────────────────────────────────────────
        // In-memory cache (lazy-loaded on first access)
        // ─────────────────────────────────────────────────────────────

        private List<Customer>? _cache;

        private List<Customer> Cache
        {
            get
            {
                if (_cache == null)
                    _cache = LoadFromDisk();
                return _cache;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // ICustomerRepository
        // ─────────────────────────────────────────────────────────────

        public IEnumerable<Customer> GetAll() => Cache.AsReadOnly();

        public void Add(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (Cache.Any(c => c.Id == customer.Id))
                throw new InvalidOperationException(
                    $"A customer with Id {customer.Id} already exists.");

            Cache.Add(customer);
        }

        public void Update(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            int index = Cache.FindIndex(c => c.Id == customer.Id);
            if (index < 0)
                throw new KeyNotFoundException(
                    $"No customer found with Id {customer.Id}.");

            Cache[index] = customer;
        }

        public void Delete(int customerId)
        {
            int removed = Cache.RemoveAll(c => c.Id == customerId);
            if (removed == 0)
                throw new KeyNotFoundException(
                    $"No customer found with Id {customerId}.");
        }

        /// <summary>
        /// Serialises the full in-memory cache to XML using atomic write:
        ///   1. Write to .tmp file
        ///   2. Rename .tmp → .xml  (atomic on the file system)
        /// Monthly expenses are intentionally omitted — they are generated
        /// at runtime and are not part of the persistent customer record.
        /// </summary>
        public void SaveChanges()
        {
            // Ensure the target directory exists
            string? dir = Path.GetDirectoryName(_xmlPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("Customers",
                    new XAttribute("version", "1.0"),
                    new XAttribute("exportedAt",
                        DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss",
                                             CultureInfo.InvariantCulture)),
                    Cache.Select(CustomerToXElement)
                )
            );

            // Atomic write: first save to .tmp, then rename
            doc.Save(_tmpPath);
            File.Move(_tmpPath, _xmlPath, overwrite: true);
        }

        // ─────────────────────────────────────────────────────────────
        // Serialisation helpers
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Loads customers from disk.
        /// Returns an empty list if the file does not yet exist (first run).
        /// Throws InvalidDataException with a user-friendly message on XML corruption.
        /// </summary>
        private List<Customer> LoadFromDisk()
        {
            if (!File.Exists(_xmlPath))
                return new List<Customer>();

            try
            {
                var doc = XDocument.Load(_xmlPath);

                return doc.Root?
                    .Elements("Customer")
                    .Select(XElementToCustomer)
                    .ToList()
                    ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"The data file '{_xmlPath}' is corrupt or has an unrecognised format.\n" +
                    $"Restore a backup or delete the file so the application can regenerate it.\n\n" +
                    $"Technical detail: {ex.Message}", ex);
            }
        }

        /// <summary>Converts a &lt;Customer&gt; XElement to a Customer object.</summary>
        private static Customer XElementToCustomer(XElement el)
        {
            int id          = int.Parse(el.Element("Id")!.Value,      CultureInfo.InvariantCulture);
            string dni      = el.Element("Dni")!.Value;
            string name     = el.Element("Name")!.Value;
            string lastName = el.Element("LastName")!.Value;
            string? email   = NullIfEmpty(el.Element("Email")?.Value);
            string phone    = el.Element("Phone")?.Value ?? "";
            DateTime dataAlta = DateTime.ParseExact(
                el.Element("DataAlta")!.Value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);

            // Monthly expenses are NOT stored in XML; the Customer constructor
            // will regenerate them randomly using its built-in seasonal algorithm.
            return new Customer(id, dni, name, lastName, email, phone, dataAlta);
        }

        /// <summary>Converts a Customer object to an XElement (without monthly expenses).</summary>
        private static XElement CustomerToXElement(Customer c)
        {
            return new XElement("Customer",
                new XElement("Id",       c.Id),
                new XElement("Dni",      c.Dni),
                new XElement("Name",     c.Name),
                new XElement("LastName", c.LastName),
                new XElement("Email",    c.Email   ?? ""),
                new XElement("Phone",    c.Phone   ?? ""),
                new XElement("DataAlta", c.DataAlta.ToString("yyyy-MM-dd",
                                            CultureInfo.InvariantCulture))
            );
        }

        private static string? NullIfEmpty(string? s)
            => string.IsNullOrEmpty(s) ? null : s;
    }
}
