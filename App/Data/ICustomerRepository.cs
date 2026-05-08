// App/Data/ICustomerRepository.cs
using System.Collections.Generic;
using iCustomer.Models;

namespace iCustomer.Data
{
    /// <summary>
    /// Contract for any customer persistence provider.
    /// Switching from XML to a database = create a new class that implements this interface.
    /// The ViewModels never need to change.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>Returns all stored customers.</summary>
        IEnumerable<Customer> GetAll();

        /// <summary>Adds a new customer. Throws InvalidOperationException if Id already exists.</summary>
        void Add(Customer customer);

        /// <summary>Updates an existing customer. Throws KeyNotFoundException if not found.</summary>
        void Update(Customer customer);

        /// <summary>Deletes a customer by Id. Throws KeyNotFoundException if not found.</summary>
        void Delete(int customerId);

        /// <summary>
        /// Persists all pending changes to the underlying storage.
        /// Always call after Add, Update, or Delete.
        /// </summary>
        void SaveChanges();
    }
}
