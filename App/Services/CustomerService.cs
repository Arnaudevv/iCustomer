// App/Services/CustomerService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using iCustomer.Data;
using iCustomer.Models;

namespace iCustomer.Services
{
    /// <summary>
    /// Business-logic layer between ViewModels and the repository.
    /// Centralises rules such as "DNI must be unique across all customers".
    ///
    /// ViewModels depend only on this class, never on ICustomerRepository directly,
    /// which keeps them decoupled from the storage mechanism.
    /// </summary>
    public class CustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ─────────────────────────────────────────────────────────────
        // Queries
        // ─────────────────────────────────────────────────────────────

        /// <summary>Returns all customers from the repository.</summary>
        public IEnumerable<Customer> GetAll() => _repository.GetAll();

        /// <summary>
        /// Returns the next available Id (max existing Id + 1).
        /// Returns 1 if no customers exist yet.
        /// </summary>
        public int GetNextId()
        {
            var all = _repository.GetAll().ToList();
            return all.Count == 0 ? 1 : all.Max(c => c.Id) + 1;
        }

        // ─────────────────────────────────────────────────────────────
        // Commands
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Adds a new customer after validating that the DNI is not already registered.
        /// Persists immediately via write-through strategy.
        /// </summary>
        /// <exception cref="ArgumentNullException">customer is null.</exception>
        /// <exception cref="InvalidOperationException">DNI already in use.</exception>
        public void AddCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            bool dniDuplicated = _repository.GetAll()
                .Any(c => string.Equals(c.Dni, customer.Dni,
                                        StringComparison.OrdinalIgnoreCase));
            if (dniDuplicated)
                throw new InvalidOperationException(
                    $"DNI '{customer.Dni}' is already registered in the system.");

            _repository.Add(customer);
            _repository.SaveChanges();   // write-through: persist immediately
        }

        /// <summary>
        /// Updates an existing customer.
        /// If the DNI changes, verifies that the new DNI is not used by another customer.
        /// Persists immediately.
        /// </summary>
        /// <exception cref="ArgumentNullException">customer is null.</exception>
        /// <exception cref="InvalidOperationException">New DNI already belongs to another customer.</exception>
        public void UpdateCustomer(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            bool dniConflict = _repository.GetAll()
                .Any(c => c.Id != customer.Id &&
                           string.Equals(c.Dni, customer.Dni,
                                         StringComparison.OrdinalIgnoreCase));
            if (dniConflict)
                throw new InvalidOperationException(
                    $"DNI '{customer.Dni}' already belongs to another customer.");

            _repository.Update(customer);
            _repository.SaveChanges();
        }

        /// <summary>
        /// Deletes a customer by Id.
        /// Persists immediately.
        /// </summary>
        public void DeleteCustomer(int customerId)
        {
            _repository.Delete(customerId);
            _repository.SaveChanges();
        }
    }
}
