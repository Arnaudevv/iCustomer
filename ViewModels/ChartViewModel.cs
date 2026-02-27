using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using iCustomer.Models;

namespace iCustomer.ViewModels
{
    class ChartViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;

        // Primary customer — the one whose chart button was clicked.
        // Changing it rebuilds the comparison list and redraws the chart.
        private Customer? _customer;
        public Customer? Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CustomerFullName));

                RefreshAvailableCustomers();

                // Reset comparison so we don't show stale data from the previous customer
                CompareCustomer = null;

                BuildChart();
            }
        }

        public string CustomerFullName =>
            _customer != null ? $"{_customer.Name} {_customer.LastName}" : "—";

        // Optional second customer selected via the ComboBox.
        // Setting it to null removes the comparison series from the chart.
        private Customer? _compareCustomer;
        public Customer? CompareCustomer
        {
            get => _compareCustomer;
            set
            {
                _compareCustomer = value;
                OnPropertyChanged();
                BuildChart();
            }
        }

        // All customers except the primary one — shown in the comparison ComboBox.
        public ObservableCollection<Customer> AvailableCustomers { get; } = new();

        private void RefreshAvailableCustomers()
        {
            AvailableCustomers.Clear();
            foreach (var c in _mainViewModel.CustomerVM.Customers)
            {
                if (c != _customer)
                    AvailableCustomers.Add(c);
            }
        }

        private SeriesCollection? _seriesCollection;
        public SeriesCollection? SeriesCollection
        {
            get => _seriesCollection;
            set { _seriesCollection = value; OnPropertyChanged(); }
        }

        public string[] MonthLabels { get; } =
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        // Formats Y-axis values as whole euros (e.g. "1 200 €").
        public Func<double, string> CurrencyFormatter { get; } =
            value => value.ToString("N0") + " €";

        public RelayCommand BackCommand { get; }

        public ChartViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            BackCommand = new RelayCommand(_ => _mainViewModel.SelectedView = "Customer");
            BuildChart();
        }

        // Builds the SeriesCollection from scratch each time the primary or
        // comparison customer changes. Adds a second series only when needed.
        private void BuildChart()
        {
            var series = new SeriesCollection
            {
                MakeSeries(
                    title:  _customer?.Name ?? "Customer",
                    values: _customer?.MonthlyExpenses ?? new double[12],
                    fill:   Color.FromRgb(0x2E, 0x41, 0x5D),
                    stroke: Color.FromRgb(0x58, 0x7B, 0xAD)
                )
            };

            if (_compareCustomer != null)
            {
                series.Add(MakeSeries(
                    title: _compareCustomer.Name,
                    values: _compareCustomer.MonthlyExpenses,
                    fill: Color.FromRgb(0x3F, 0x6B, 0x35),
                    stroke: Color.FromRgb(0x3F, 0xB9, 0x50)
                ));
            }

            SeriesCollection = series;
        }

        // Helper that creates a single styled ColumnSeries.
        private static ColumnSeries MakeSeries(string title, double[] values,
                                               Color fill, Color stroke)
        {
            return new ColumnSeries
            {
                Title = title,
                Values = new ChartValues<double>(values),
                Fill = new SolidColorBrush(fill),
                Stroke = new SolidColorBrush(stroke),
                StrokeThickness = 1,
                DataLabels = false,
                MaxColumnWidth = 30
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}