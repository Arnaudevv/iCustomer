using System.ComponentModel;
using System.Runtime.CompilerServices;
using iCustomer.Views;

namespace iCustomer.ViewModels
{
    // Central ViewModel. Owns all sub-ViewModels and controls which view
    // is currently displayed inside the main window.
    class MainViewModel : INotifyPropertyChanged
    {
        public HomeViewModel HomeVM { get; set; }
        public CustomerViewModel CustomerVM { get; set; }
        public FormulariClientViewModel FormulariClientVM { get; set; }
        public ChartViewModel ChartVM { get; set; }

        // The actual UserControl/Window instance shown in the ContentControl.
        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Setting this triggers ChangeView(), which swaps CurrentView.
        private string? _selectedView;
        public string? SelectedView
        {
            get { return _selectedView; }
            set
            {
                _selectedView = value;
                OnPropertyChanged();
                ChangeView();
            }
        }

        public MainViewModel()
        {
            CustomerVM = new CustomerViewModel(this);
            FormulariClientVM = new FormulariClientViewModel(this);
            HomeVM = new HomeViewModel(this);
            ChartVM = new ChartViewModel(this);

            // Start on the customer list
            SelectedView = "Customer";
            ChangeView();
        }

        // Creates a new instance of the requested view and binds it to
        // the matching ViewModel. ContentControl in MainWindow renders it.
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
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}