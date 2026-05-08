// App/App.xaml.cs  — UPDATED
using System.IO;
using System.Windows;

namespace iCustomer
{
    /// <summary>
    /// Application entry point.
    ///
    /// CHANGE: Added a global DispatcherUnhandledException handler.
    /// This catches exceptions that escape all ViewModel try/catch blocks
    /// and presents a user-friendly message instead of crashing silently.
    ///
    /// Specifically handles InvalidDataException (corrupt XML) with a
    /// clear message guiding the user on how to recover.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Catch unhandled exceptions on the UI thread
            DispatcherUnhandledException += (s, args) =>
            {
                string message = args.Exception is InvalidDataException
                    ? args.Exception.Message
                    : $"Unexpected error: {args.Exception.Message}";

                MessageBox.Show(
                    message,
                    "Critical Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                args.Handled = true; // Prevent abrupt shutdown
            };

            base.OnStartup(e);
        }
    }
}