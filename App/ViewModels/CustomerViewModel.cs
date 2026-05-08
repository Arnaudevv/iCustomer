using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using FastReport;
using FastReport.Export.PdfSimple;
using iCustomer.Models;
using iCustomer.Services;
using iCustomer.Views;

namespace iCustomer.ViewModels
{
    class CustomerViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly CustomerService _customerService;

        // The list bound to the DataGrid.
        // ObservableCollection notifies the UI automatically on add/remove.
        public ObservableCollection<Customer> Customers { get; } =
            new ObservableCollection<Customer>();

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCustomerCommand { get; }
        public RelayCommand EditCustomerCommand { get; }
        public RelayCommand DelCustomerCommand { get; }
        public RelayCommand ShowCustomerChart { get; }

        public RelayCommand GenerateReportCommand { get; }
        public RelayCommand GenerateGeneralReportCommand { get; }

        public CustomerViewModel(MainViewModel mainViewModel,
                                 CustomerService customerService)
        {
            _mainViewModel = mainViewModel;
            _customerService = customerService;

            // Load from XML repository (or empty list on first run)
            LoadCustomersFromRepository();

            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
            EditCustomerCommand = new RelayCommand(x => EditCustomer(x));
            DelCustomerCommand = new RelayCommand(x => DeleteCustomer(x));
            ShowCustomerChart = new RelayCommand(x => ShowChart(x));
            GenerateReportCommand = new RelayCommand(x => GenerateReport(x));
            GenerateGeneralReportCommand = new RelayCommand(x => GenerateGeneralReport(x));
        }

        // ─────────────────────────────────────────────────────────────
        // Repository interaction
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Clears the UI collection and reloads it from the repository.
        /// Called at startup and can be called after any external change.
        /// </summary>
        public void LoadCustomersFromRepository()
        {
            Customers.Clear();
            foreach (var customer in _customerService.GetAll())
                Customers.Add(customer);
        }

        // ─────────────────────────────────────────────────────────────
        // Command implementations
        // ─────────────────────────────────────────────────────────────

        private void AddCustomer()
        {
            _mainViewModel.FormulariClientVM.PrepareForAdd(_customerService.GetNextId());
            _mainViewModel.SelectedView = "FormulariClient";
        }

        private void EditCustomer(object? param)
        {
            if (param is Customer customer)
            {
                _mainViewModel.FormulariClientVM.PrepareForEdit(customer);
                _mainViewModel.SelectedView = "FormulariClient";
            }
        }

        private void DeleteCustomer(object? param)
        {
            if (param is Customer customer)
            {
                var dialog = new DeleteCustomerModal(
                    $"Are you sure you want to delete {customer.Name} {customer.LastName}?");

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        _customerService.DeleteCustomer(customer.Id);
                        Customers.Remove(customer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Could not delete the customer.\n\n{ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ShowChart(object? param)
        {
            if (param is Customer customer)
            {
                _mainViewModel.ChartVM.Customer = customer;
                _mainViewModel.SelectedView = "Chart";
            }
        }

        /// <summary>
        /// Generates the individual report for the selected customer.
        /// </summary>
        /// 

        // This function allows searching for the project root directory to save the reports there.
        private static string? FindProjectRoot(string startDir)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null)
            {
                if (dir.GetFiles("*.sln").Length > 0)
                    return dir.FullName;
                dir = dir.Parent;
            }
            return null;
        }
        private void GenerateReport(object? param)
        {
            var customer = param as Customer ?? SelectedCustomer;
            if (customer == null)
            {
                MessageBox.Show("Please select a customer first.",
                                "No customer selected",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }

            try
            {
                string reportsDir = Path.Combine(AppContext.BaseDirectory, "Reports");
                string templatePath = Path.Combine(reportsDir, "Individual_Report.frx");

                // Reports are stored in the `PDF_Reports` directory,
                // located at the project root using the `FindProjectRoot` function.
                string? root = FindProjectRoot(AppContext.BaseDirectory);
                string pdfDir = Path.Combine(root ?? AppContext.BaseDirectory, "PDF_Reports");
                Directory.CreateDirectory(pdfDir);
                string outputPath = Path.Combine(pdfDir, "InformeClient.pdf");

                var report = new Report();
                report.Load(templatePath);

                // Force System.Windows.Forms.dll to load into the AppDomain so FastReport's 
                // Roslyn compiler can find it and resolve CS0234.
                var _ = typeof(System.Windows.Forms.Form);

                // Fix 1: Clear only connections and data sources to keep SystemVariables (like Date) intact
                report.Dictionary.Connections.Clear();
                report.Dictionary.DataSources.Clear();

                // Register the single customer as a collection to bind to the "Customer" data source
                report.RegisterData(new[] { customer }, "Customer");
                var dataSource = report.GetDataSource("Customer");
                if (dataSource != null)
                {
                    dataSource.Enabled = true;
                    if (report.FindObject("Data1") is FastReport.DataBand dataBand)
                    {
                        dataBand.DataSource = dataSource;
                    }
                }

                // Fix 2: Remove the default Windows Forms using from the generated script
                if (!string.IsNullOrEmpty(report.ScriptText))
                {
                    report.ScriptText = report.ScriptText.Replace("using System.Windows.Forms;", "");
                }

                report.Prepare();

                using (var ms = new MemoryStream())
                {
                    var pdfExport = new PDFSimpleExport();
                    report.Export(pdfExport, ms);
                    File.WriteAllBytes(outputPath, ms.ToArray());
                }

                // Open the generated PDF automatically
                Process.Start(new ProcessStartInfo
                {
                    FileName = outputPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to generate report.\n\n{ex.Message}",
                    "Export Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Generates the global report for all customers.
        /// </summary>
        private void GenerateGeneralReport(object? param)
        {
            try
            {
                string reportsDir = Path.Combine(AppContext.BaseDirectory, "Reports");
                string templatePath = Path.Combine(reportsDir, "InformeGlobal.frx");

                // Reports are stored in the `PDF_Reports` directory,
                // located at the project root using the `FindProjectRoot` function.
                string? root = FindProjectRoot(AppContext.BaseDirectory);
                string pdfDir = Path.Combine(root ?? AppContext.BaseDirectory, "PDF_Reports");
                Directory.CreateDirectory(pdfDir);
                string outputPath = Path.Combine(pdfDir, "InformeGeneral.pdf");

                var report = new Report();
                report.Load(templatePath);

                // Force System.Windows.Forms.dll to load into the AppDomain so FastReport's 
                // Roslyn compiler can find it and resolve CS0234.
                var _ = typeof(System.Windows.Forms.Form);

                // Fix 1: Clear only connections and data sources to keep SystemVariables (like Date) intact
                report.Dictionary.Connections.Clear();
                report.Dictionary.DataSources.Clear();

                // Register the full customer collection
                // Converting to List to ensure FastReport enumerates it correctly
                report.RegisterData(Customers.ToList(), "Customer");
                var dataSource = report.GetDataSource("Customer");
                if (dataSource != null)
                {
                    dataSource.Enabled = true;
                    if (report.FindObject("Data1") is FastReport.DataBand dataBand)
                    {
                        dataBand.DataSource = dataSource;
                    }
                }

                // Fix 2: Remove the default Windows Forms using from the generated script
                if (!string.IsNullOrEmpty(report.ScriptText))
                {
                    report.ScriptText = report.ScriptText.Replace("using System.Windows.Forms;", "");
                }

                report.Prepare();

                using (var ms = new MemoryStream())
                {
                    var pdfExport = new PDFSimpleExport();
                    report.Export(pdfExport, ms);
                    File.WriteAllBytes(outputPath, ms.ToArray());
                }

                // Open the generated PDF automatically
                Process.Start(new ProcessStartInfo
                {
                    FileName = outputPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to generate general report.\n\n{ex.Message}",
                    "Export Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // INotifyPropertyChanged
        // ─────────────────────────────────────────────────────────────

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}