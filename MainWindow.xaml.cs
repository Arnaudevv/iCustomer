using System.Windows;
using iCustomer.ViewModels;

namespace iCustomer
{
    // Entry point window. Acts as the SPA shell — it never reloads,
    // just swaps the inner view via MainViewModel.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}