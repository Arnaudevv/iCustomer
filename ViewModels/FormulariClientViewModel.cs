using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using iCustomer.Models;

namespace iCustomer.ViewModels
{
    // Handles both Add and Edit — the form is the same, we just check
    // whether the ID already exists in the collection when saving.
    class FormulariClientViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string? _dni;
        public string? Dni
        {
            get { return _dni; }
            set { _dni = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _firstName;
        public string? FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _lastName;
        public string? LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _email;
        public string? Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); ValidateForm(); }
        }

        private string? _phone;
        public string? Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(); ValidateForm(); }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { _registrationDate = value; OnPropertyChanged(); }
        }

        // Controls whether the Save button is enabled.
        private bool _isFormValid;
        public bool IsFormValid
        {
            get { return _isFormValid; }
            set { _isFormValid = value; OnPropertyChanged(); }
        }

        // Error messages shown below each field — null means the field is valid.
        private string? _firstNameError;
        public string? FirstNameError
        {
            get => _firstNameError;
            set { _firstNameError = value; OnPropertyChanged(); }
        }

        private string? _lastNameError;
        public string? LastNameError
        {
            get => _lastNameError;
            set { _lastNameError = value; OnPropertyChanged(); }
        }

        private string? _dniError;
        public string? DniError
        {
            get => _dniError;
            set { _dniError = value; OnPropertyChanged(); }
        }

        private string? _emailError;
        public string? EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(); }
        }

        private string? _phoneError;
        public string? PhoneError
        {
            get => _phoneError;
            set { _phoneError = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public FormulariClientViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SaveCommand = new RelayCommand(x => Save());
            CancelCommand = new RelayCommand(x => Cancel());
            RegistrationDate = DateTime.Now;
            ValidateForm();
        }

        // Runs on every field change. Sets individual error messages and
        // updates IsFormValid so the Save button reacts in real time.
        private void ValidateForm()
        {
            FirstNameError = string.IsNullOrWhiteSpace(FirstName) ? "Name is required." :
                             FirstName.Length < 3 ? "Name must be at least 3 characters." : null;

            LastNameError = string.IsNullOrWhiteSpace(LastName) ? "Surname is required." :
                            LastName.Length < 3 ? "Surname must be at least 3 characters." : null;

            DniError = !ValidateDni(Dni) ? "DNI must be 8 digits followed by a letter (e.g. 12345678Z)." : null;

            EmailError = !ValidateEmail(Email) ? "Enter a valid email address (e.g. user@domain.com)." : null;

            PhoneError = string.IsNullOrWhiteSpace(Phone) ? "Phone is required." :
                         !Regex.IsMatch(Phone, @"^\d+$") ? "Phone must contain only digits." :
                         Phone.Length < 9 ? "Phone must be at least 9 digits." : null;

            IsFormValid = FirstNameError == null && LastNameError == null &&
                          DniError == null && EmailError == null && PhoneError == null;
        }

        // Spanish DNI format: exactly 8 digits followed by one letter.
        private bool ValidateDni(string? dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return false;

            string pattern = @"^\d{8}[A-Za-z]$";
            return Regex.IsMatch(dni, pattern);
        }

        // Basic email check: requires something@something.something.
        private bool ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@]+@[^@]+\.[^@]+$";
            return Regex.IsMatch(email, pattern);
        }

        // If the ID already exists, update the existing customer in place.
        // Otherwise, create a new one and add it to the collection.
        private void Save()
        {
            if (!IsFormValid)
                return;

            var existingCustomer = _mainViewModel.CustomerVM.Customers.FirstOrDefault(c => c.Id == Id);

            if (existingCustomer != null)
            {
                existingCustomer.Dni = Dni ?? "";
                existingCustomer.Name = FirstName ?? "";
                existingCustomer.LastName = LastName ?? "";
                existingCustomer.Email = Email;
                existingCustomer.Phone = Phone ?? "";
                existingCustomer.DataAlta = RegistrationDate;
            }
            else
            {
                var newCustomer = new Customer(
                    Id,
                    Dni ?? "",
                    FirstName ?? "",
                    LastName ?? "",
                    Email,
                    Phone ?? "",
                    RegistrationDate
                );

                _mainViewModel.CustomerVM.Customers.Add(newCustomer);
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}