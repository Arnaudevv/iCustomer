using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using iCustomer.Models;
using iCustomer.Services;

namespace iCustomer.ViewModels
{
    /// <summary>
    /// Handles both Add and Edit operations — the form is the same in both cases.
    ///
    /// CHANGE: The ViewModel now receives a CustomerService via constructor injection
    /// and calls AddCustomer / UpdateCustomer on save, which persists data to XML.
    /// The form no longer needs to touch the ObservableCollection directly for new adds;
    /// that is done via LoadCustomersFromRepository() called on return to CustomerView.
    /// </summary>
    class FormulariClientViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly CustomerService _customerService;

        // ─────────────────────────────────────────────────────────────
        // Form fields
        // ─────────────────────────────────────────────────────────────

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        private string? _dni;
        public string? Dni
        {
            get => _dni;
            set { _dni = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _firstName;
        public string? FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _lastName;
        public string? LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _email;
        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); ValidateForm(); }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set { _registrationDate = value; OnPropertyChanged(); }
        }

        // ─────────────────────────────────────────────────────────────
        // Validation flags (set by custom controls in the DLL project)
        // ─────────────────────────────────────────────────────────────

        private bool _isFormValid;
        public bool IsFormValid
        {
            get => _isFormValid;
            set { _isFormValid = value; OnPropertyChanged(); }
        }

        private bool _firstNameValid;
        public bool FirstNameValid
        {
            get => _firstNameValid;
            set { _firstNameValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        private bool _lastNameValid;
        public bool LastNameValid
        {
            get => _lastNameValid;
            set { _lastNameValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        private bool _emailValid;
        public bool EmailValid
        {
            get => _emailValid;
            set { _emailValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        private bool _dniValid;
        public bool DniValid
        {
            get => _dniValid;
            set { _dniValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        private bool _phoneValid;
        public bool PhoneValid
        {
            get => _phoneValid;
            set { _phoneValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        // ─────────────────────────────────────────────────────────────
        // Commands
        // ─────────────────────────────────────────────────────────────

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        // ─────────────────────────────────────────────────────────────
        // Constructor
        // ─────────────────────────────────────────────────────────────

        public FormulariClientViewModel(MainViewModel mainViewModel,
                                        CustomerService customerService)
        {
            _mainViewModel = mainViewModel;
            _customerService = customerService;

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
            RegistrationDate = DateTime.Now;
            ValidateForm();
        }

        // ─────────────────────────────────────────────────────────────
        // Form preparation (called from CustomerViewModel)
        // ─────────────────────────────────────────────────────────────

        /// <summary>Prepares the form for adding a new customer.</summary>
        public void PrepareForAdd(int nextId)
        {
            Id = nextId;
            FirstName = "";
            LastName = "";
            Dni = "";
            Email = "";
            Phone = "";
            RegistrationDate = DateTime.Now.Date;
        }

        /// <summary>Prepares the form for editing an existing customer.</summary>
        public void PrepareForEdit(Customer customer)
        {
            Id = customer.Id;
            FirstName = customer.Name;
            LastName = customer.LastName;
            Dni = customer.Dni;
            Email = customer.Email;
            Phone = customer.Phone;
            RegistrationDate = customer.DataAlta;
        }

        // ─────────────────────────────────────────────────────────────
        // Private logic
        // ─────────────────────────────────────────────────────────────

        private void ValidateForm()
        {
            IsFormValid = FirstNameValid && LastNameValid &&
                          DniValid && EmailValid && PhoneValid;
        }

        private void Save()
        {
            if (!IsFormValid) return;

            // Determine whether this is an add or an edit
            bool isExisting = false;
            foreach (var c in _mainViewModel.CustomerVM.Customers)
            {
                if (c.Id == Id) { isExisting = true; break; }
            }

            try
            {
                if (isExisting)
                {
                    // ── Edit ─────────────────────────────────────────────────────
                    // Find the existing object (same reference that the UI is bound to)
                    // and mutate it in place so the DataGrid updates without a full reload.
                    Customer? existing = null;
                    foreach (var c in _mainViewModel.CustomerVM.Customers)
                    {
                        if (c.Id == Id) { existing = c; break; }
                    }

                    if (existing != null)
                    {
                        existing.Dni = Dni ?? "";
                        existing.Name = FirstName ?? "";
                        existing.LastName = LastName ?? "";
                        existing.Email = Email;
                        existing.Phone = Phone ?? "";
                        existing.DataAlta = RegistrationDate;

                        _customerService.UpdateCustomer(existing);
                        // UI is already updated because 'existing' is the same
                        // reference that is in the ObservableCollection.
                    }
                }
                else
                {
                    // ── Add ──────────────────────────────────────────────────────
                    var newCustomer = new Customer(
                        Id,
                        Dni ?? "",
                        FirstName ?? "",
                        LastName ?? "",
                        Email,
                        Phone ?? "",
                        RegistrationDate);

                    _customerService.AddCustomer(newCustomer);
                    _mainViewModel.CustomerVM.Customers.Add(newCustomer);
                }
            }
            catch (InvalidOperationException ex)
            {
                // Duplicate DNI or other business-rule violation
                MessageBox.Show(ex.Message, "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Stay on the form
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save the customer.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClearForm();
            _mainViewModel.SelectedView = "Customer";
        }

        private void Cancel()
        {
            ClearForm();
            _mainViewModel.SelectedView = "Customer";
        }

        private void ClearForm()
        {
            Id = 0;
            Dni = "";
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            RegistrationDate = DateTime.Now;
            ValidateForm();
        }

        // ─────────────────────────────────────────────────────────────
        // INotifyPropertyChanged
        // ─────────────────────────────────────────────────────────────

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}