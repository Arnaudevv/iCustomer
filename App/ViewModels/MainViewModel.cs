using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using iCustomer.Data;
using iCustomer.Services;
using iCustomer.Views;

namespace iCustomer.ViewModels
{
    /// <summary>
    /// Central ViewModel and composition root.
    /// Owns all sub-ViewModels and controls which view is visible inside the main window.
    ///
    /// CHANGE: This class now also owns the infrastructure objects (repository + service)
    /// and injects them into the ViewModels that need them. This keeps dependency
    /// creation in one place and makes future swaps (e.g. to a database) trivial.
    /// </summary>
    class MainViewModel : INotifyPropertyChanged
    {
        public HomeViewModel HomeVM { get; }
        public CustomerViewModel CustomerVM { get; }
        public FormulariClientViewModel FormulariClientVM { get; }
        public ChartViewModel ChartVM { get; }

        // The actual UserControl instance shown in the ContentControl.
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Setting this triggers ChangeView(), which swaps CurrentView.
        private string? _selectedView;
        public string? SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
                ChangeView();
            }
        }

        public MainViewModel()
        {
            // ── Build the XML path (next to the executable) ────────────────────
            // For production installs under "Program Files" consider using:
            //   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            string appDir = AppContext.BaseDirectory;
            string xmlPath = Path.Combine(appDir, "Data", "customers.xml");

            // ── Compose the infrastructure (done once, here, in the root) ──────
            ICustomerRepository repository = new XmlCustomerRepository(xmlPath);
            var customerService = new CustomerService(repository);

            // ── Instantiate ViewModels with dependency injection ─────────────
            HomeVM = new HomeViewModel(this);
            ChartVM = new ChartViewModel(this);
            CustomerVM = new CustomerViewModel(this, customerService);
            FormulariClientVM = new FormulariClientViewModel(this, customerService);

            // Start on the customer list
            SelectedView = "Customer";
        }

        // Creates a new instance of the requested view and wires its DataContext.
        private void ChangeView()
        {
            switch (SelectedView)
            {
                case "Home": CurrentView = new HomeView { DataContext = HomeVM }; break;
                case "Customer": CurrentView = new CustomerView { DataContext = CustomerVM }; break;
                case "FormulariClient": CurrentView = new FormulariClientView { DataContext = FormulariClientVM }; break;
                case "Chart": CurrentView = new ChartView { DataContext = ChartVM }; break;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}