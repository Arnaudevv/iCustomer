using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iCustomer.Models;
using iCustomer.Views;

namespace iCustomer.ViewModels
{
    class CustomerViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;

        // The list bound to the DataGrid — ObservableCollection updates the UI automatically.
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCustomerCommand { get; set; }
        public RelayCommand EditCustomerCommand { get; set; }
        public RelayCommand DelCustomerCommand { get; set; }
        public RelayCommand ShowCustomerChart { get; set; }

        public CustomerViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            // Seed data — stands in for a real database
            Customers.Add(new Customer(1, "12345678Z", "Jordi", "Pérez", "jordi@email.com", "600000000", new System.DateTime(2024, 10, 2)));
            Customers.Add(new Customer(2, "12332167Z", "Marc", "Molist", "marc.molist@email.com", "600000000", new System.DateTime(2024, 10, 1)));
            Customers.Add(new Customer(3, "87654321A", "Laura", "García", "laura.garcia@email.com", "611223344", new System.DateTime(2024, 10, 3)));
            Customers.Add(new Customer(4, "11223344B", "Joan", "Martí", "joan.marti@email.com", "622334455", new System.DateTime(2024, 10, 4)));
            Customers.Add(new Customer(5, "55667788C", "Maria", "Vila", "maria.vila@email.com", "633445566", new System.DateTime(2024, 10, 5)));
            Customers.Add(new Customer(6, "99887766D", "Carlos", "López", "carlos.lopez@email.com", "644556677", new System.DateTime(2024, 10, 6)));
            Customers.Add(new Customer(7, "44556677E", "Anna", "Sánchez", "anna.sanchez@email.com", "655667788", new System.DateTime(2024, 10, 7)));
            Customers.Add(new Customer(8, "22334455F", "Pau", "Costa", "pau.costa@email.com", "666778899", new System.DateTime(2024, 10, 8)));
            Customers.Add(new Customer(9, "66778899G", "Marta", "Ferrer", "marta.ferrer@email.com", "677889900", new System.DateTime(2024, 10, 9)));
            Customers.Add(new Customer(10, "33445566H", "David", "Rodríguez", "david.rodriguez@email.com", "688990011", new System.DateTime(2024, 10, 10)));
            Customers.Add(new Customer(11, "77889900J", "Elena", "Fernández", "elena.fernandez@email.com", "699001122", new System.DateTime(2024, 10, 11)));
            Customers.Add(new Customer(12, "10293847K", "Albert", "Puig", "albert.puig@email.com", "610112233", new System.DateTime(2024, 10, 12)));
            Customers.Add(new Customer(13, "56473829L", "Sílvia", "Gómez", "silvia.gomez@email.com", "620223344", new System.DateTime(2024, 10, 13)));
            Customers.Add(new Customer(14, "91827364M", "Pol", "Serra", "pol.serra@email.com", "630334455", new System.DateTime(2024, 10, 14)));
            Customers.Add(new Customer(15, "19283746N", "Cristina", "Díaz", "cristina.diaz@email.com", "640445566", new System.DateTime(2024, 10, 15)));
            Customers.Add(new Customer(16, "51627384P", "Xavier", "Muñoz", "xavier.munoz@email.com", "650556677", new System.DateTime(2024, 10, 16)));
            Customers.Add(new Customer(17, "95847362Q", "Laia", "Roca", "laia.roca@email.com", "660667788", new System.DateTime(2024, 10, 17)));
            Customers.Add(new Customer(18, "15263748R", "Daniel", "González", "daniel.gonzalez@email.com", "670778899", new System.DateTime(2024, 10, 18)));
            Customers.Add(new Customer(19, "59687758S", "Núria", "Font", "nuria.font@email.com", "680889900", new System.DateTime(2024, 10, 19)));
            Customers.Add(new Customer(20, "93847561T", "Àlex", "Ruiz", "alex.ruiz@email.com", "690990011", new System.DateTime(2024, 10, 20)));
            Customers.Add(new Customer(21, "37485960V", "Teresa", "Vidal", "teresa.vidal@email.com", "612345678", new System.DateTime(2024, 10, 21)));
            Customers.Add(new Customer(22, "82736455W", "Víctor", "Torres", "victor.torres@email.com", "698765432", new System.DateTime(2024, 10, 22)));
            Customers.Add(new Customer(23, "11224466X", "Miquel", "Serrano", "miquel.serrano@email.com", "622112233", new System.DateTime(2024, 10, 23)));
            Customers.Add(new Customer(24, "55668800Y", "Irene", "Molina", "irene.molina@email.com", "633223344", new System.DateTime(2024, 10, 24)));
            Customers.Add(new Customer(25, "99881122Z", "Oscar", "Blanco", "oscar.blanco@email.com", "644334455", new System.DateTime(2024, 10, 25)));
            Customers.Add(new Customer(26, "44552233A", "Sara", "Castillo", "sara.castillo@email.com", "655445566", new System.DateTime(2024, 10, 26)));
            Customers.Add(new Customer(27, "22339988B", "Roger", "Navarro", "roger.navarro@email.com", "666556677", new System.DateTime(2024, 10, 27)));
            Customers.Add(new Customer(28, "66774455C", "Julia", "Domènech", "julia.domenech@email.com", "677667788", new System.DateTime(2024, 10, 28)));
            Customers.Add(new Customer(29, "33441122D", "Gabriel", "Ortega", "gabriel.ortega@email.com", "688778899", new System.DateTime(2024, 10, 29)));
            Customers.Add(new Customer(30, "77885566E", "Alicia", "Rubio", "alicia.rubio@email.com", "699889900", new System.DateTime(2024, 10, 30)));
            Customers.Add(new Customer(31, "10294857F", "Hugo", "Marín", "hugo.marin@email.com", "610990011", new System.DateTime(2024, 10, 31)));
            Customers.Add(new Customer(32, "56472910G", "Paula", "Iglesias", "paula.iglesias@email.com", "620001122", new System.DateTime(2024, 11, 1)));

            AddCustomerCommand = new RelayCommand(x => AddCustomer());
            EditCustomerCommand = new RelayCommand(x => EditCustomer(x));
            DelCustomerCommand = new RelayCommand(x => DelCustomer(x));
            ShowCustomerChart = new RelayCommand(x => ShowChart(x));
        }

        // Finds the current highest ID and adds 1 to avoid duplicates.
        private int GetNextId()
        {
            int maxId = 0;
            foreach (var customer in Customers)
            {
                if (customer.Id > maxId)
                    maxId = customer.Id;
            }
            return maxId + 1;
        }

        // Pre-fills the form with empty values and a new ID, then navigates to it.
        private void AddCustomer()
        {
            _mainViewModel.FormulariClientVM.Id = GetNextId();
            _mainViewModel.FormulariClientVM.FirstName = "";
            _mainViewModel.FormulariClientVM.LastName = "";
            _mainViewModel.FormulariClientVM.Dni = "";
            _mainViewModel.FormulariClientVM.Email = "";
            _mainViewModel.FormulariClientVM.Phone = "";
            _mainViewModel.FormulariClientVM.RegistrationDate = System.DateTime.Now.Date;
            _mainViewModel.SelectedView = "FormulariClient";
        }

        // Loads the selected customer's data into the form for editing.
        private void EditCustomer(object? param)
        {
            if (param is Customer customer)
            {
                _mainViewModel.FormulariClientVM.Id = customer.Id;
                _mainViewModel.FormulariClientVM.FirstName = customer.Name;
                _mainViewModel.FormulariClientVM.LastName = customer.LastName;
                _mainViewModel.FormulariClientVM.Dni = customer.Dni;
                _mainViewModel.FormulariClientVM.Email = customer.Email;
                _mainViewModel.FormulariClientVM.Phone = customer.Phone;
                _mainViewModel.FormulariClientVM.RegistrationDate = customer.DataAlta;
                _mainViewModel.SelectedView = "FormulariClient";
            }
        }

        // Shows a confirmation dialog before deleting — avoids accidental removals.
        private void DelCustomer(object? param)
        {
            if (param is Customer customer)
            {
                var dialog = new DeleteCustomerModal($"Are you sure you want to delete {customer.Name} {customer.LastName}?");
                if (dialog.ShowDialog() == true)
                    Customers.Remove(customer);
            }
        }

        // Passes the chosen customer to ChartViewModel and navigates to the chart view.
        private void ShowChart(object? param)
        {
            if (param is Customer customer)
            {
                _mainViewModel.ChartVM.Customer = customer;
                _mainViewModel.SelectedView = "Chart";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}