using System.Windows.Controls;

namespace iCustomer.Views
{
    // UserControl instead of Window because this view is swapped inside
    // MainWindow at runtime — no separate window needed (SPA pattern).
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
        }
    }
}