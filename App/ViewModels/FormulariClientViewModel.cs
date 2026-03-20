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

        // Receives the first name validation to control the Save button in real time.
        private bool _firstNameValid;
        public bool FirstNameValid
        {
            get => _firstNameValid;
            set { _firstNameValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        // Receives the last name validation to control the Save button in real time.
        private bool _lastNameValid;
        public bool LastNameValid
        {
            get => _lastNameValid;
            set { _lastNameValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        // Receives the email validation to control the Save button in real time.
        private bool _emailValid;
        public bool EmailValid
        {
            get => _emailValid;
            set { _emailValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        // Receives the DNI validation to control the Save button in real time.
        private bool _dniValid;
        public bool DniValid
        {
            get => _dniValid;
            set { _dniValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        // Receives the phone validation to control the Save button in real time.
        private bool _phoneValid;
        public bool PhoneValid
        {
            get => _phoneValid;
            set { _phoneValid = value; OnPropertyChanged(); ValidateForm(); }
        }

        // Runs on every field change and updates IsFormValid so the Save button reacts in real time.
        private void ValidateForm()
        {
            IsFormValid = FirstNameValid && LastNameValid &&
                          DniValid && EmailValid && PhoneValid;
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