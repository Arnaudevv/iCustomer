// App/Services/CustomerReportXmlExporter.cs
using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using iCustomer.Models;

namespace iCustomer.Services
{
    /// <summary>
    /// Generates a single-customer XML file suitable for use as a
    /// FastReports XmlDataSource.
    ///
    /// DESIGN NOTES
    /// ─────────────
    /// • Monthly expenses are intentionally excluded (per project requirements).
    ///   Only the customer's personal/contact data is exported.
    /// • Each call overwrites the previous file for the same customer (deterministic filename).
    /// • The exported file is placed in the "Reports" sub-folder next to the executable
    ///   so that FastReports can locate it at runtime.
    ///
    /// FASTREPORTS INTEGRATION NOTES
    /// ──────────────────────────────
    /// The generated XML has this structure:
    ///
    ///   &lt;Customers exportedAt="..." customerId="..."&gt;
    ///     &lt;Customer&gt;
    ///       &lt;Id&gt;1&lt;/Id&gt;
    ///       &lt;Dni&gt;12345678Z&lt;/Dni&gt;
    ///       &lt;Name&gt;Jordi&lt;/Name&gt;
    ///       ...
    ///     &lt;/Customer&gt;
    ///   &lt;/Customers&gt;
    ///
    /// In FastReports, set the XmlDataSource path to this file's location.
    /// The fields map directly: [Customer.Id], [Customer.Name], etc.
    /// </summary>
    public static class CustomerReportXmlExporter
    {
        // ─────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Exports a single customer's data to an XML file and returns the full path.
        /// The file is overwritten on each call.
        /// </summary>
        /// <param name="customer">The customer to export.</param>
        /// <param name="outputDirectory">
        ///   Directory where the XML will be written.
        ///   Typically: Path.Combine(AppContext.BaseDirectory, "Reports")
        /// </param>
        /// <returns>Full path to the generated XML file.</returns>
        public static string ExportCustomer(Customer customer, string outputDirectory)
        {
            if (customer == null)  throw new ArgumentNullException(nameof(customer));
            if (outputDirectory == null) throw new ArgumentNullException(nameof(outputDirectory));

            Directory.CreateDirectory(outputDirectory);

            string fileName = BuildFileName(customer);
            string filePath = Path.Combine(outputDirectory, fileName);

            var doc = BuildDocument(customer);
            doc.Save(filePath);

            return filePath;
        }

        // ─────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns a deterministic, filesystem-safe filename for the customer's report XML.
        /// Example: "customer_7_SilGom.xml"
        /// </summary>
        private static string BuildFileName(Customer customer)
        {
            // Use a short suffix derived from name for human readability
            string namePart = Sanitise(
                (customer.Name.Length  >= 3 ? customer.Name[..3]     : customer.Name) +
                (customer.LastName.Length >= 3 ? customer.LastName[..3] : customer.LastName));

            return $"customer_{customer.Id}_{namePart}.xml";
        }

        /// <summary>Removes characters that are unsafe in file names.</summary>
        private static string Sanitise(string s)
        {
            var invalid = Path.GetInvalidFileNameChars();
            foreach (char c in invalid)
                s = s.Replace(c.ToString(), "");
            return s;
        }

        /// <summary>
        /// Builds the XDocument for a single customer — without monthly expenses.
        /// </summary>
        private static XDocument BuildDocument(Customer customer)
        {
            return new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("Customers",
                    new XAttribute("version",    "1.0"),
                    new XAttribute("exportedAt",
                        DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss",
                                             CultureInfo.InvariantCulture)),
                    new XAttribute("customerId", customer.Id),

                    new XElement("Customer",
                        new XElement("Id",       customer.Id),
                        new XElement("Dni",      customer.Dni),
                        new XElement("Name",     customer.Name),
                        new XElement("LastName", customer.LastName),
                        new XElement("FullName", $"{customer.Name} {customer.LastName}"),
                        new XElement("Email",    customer.Email   ?? ""),
                        new XElement("Phone",    customer.Phone   ?? ""),
                        new XElement("DataAlta",
                            customer.DataAlta.ToString("yyyy-MM-dd",
                                CultureInfo.InvariantCulture)),
                        new XElement("DataAltaFormatted",
                            customer.DataAlta.ToString("dd/MM/yyyy",
                                CultureInfo.InvariantCulture))
                    )
                )
            );
        }
    }
}
