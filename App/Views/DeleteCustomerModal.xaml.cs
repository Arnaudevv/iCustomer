using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace iCustomer.Views
{
    // Simple confirmation dialog. Uses DialogResult so the caller can
    // check whether the user clicked Confirm or Cancel.
    public partial class DeleteCustomerModal : Window
    {
        public string Message { get; set; }

        public DeleteCustomerModal(string message)
        {
            Message = message;
            DataContext = this;  // Binds Message directly to the XAML
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}