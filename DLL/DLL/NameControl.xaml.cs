using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;


namespace DLL
{
    /// <summary>
    /// Interaction logic for NameControl.xaml
    /// </summary>
    [ToolboxItem(true)]
    public partial class NameControl : UserControl
    {
        public NameControl() => InitializeComponent();

        // DependencyProperty for the client's name (WPF Property)
        public static readonly DependencyProperty CustomerNameProperty = DependencyProperty.Register(
            "CustomerName",
            typeof(string),
            typeof(NameControl),
            new PropertyMetadata(string.Empty));

        public string CustomerName
        {
            get { return (string)GetValue(CustomerNameProperty); }
            set { SetValue(CustomerNameProperty, value); }
        }

        // DependencyProperty for the minimum length mask (WPF Property)
        public static readonly DependencyProperty MinLengthProperty = DependencyProperty.Register(
            "MinLength",
            typeof(int),
            typeof(NameControl),
            new PropertyMetadata(3));

        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        // Boolean DependencyProperty to indicate whether the text is valid or not (WPF Property)
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(
            "IsValid",
            typeof(Boolean),
            typeof(NameControl),
            new PropertyMetadata(false));

        public Boolean IsValid
        {
            get { return (Boolean)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // DependencyProperty to modify the border color when empty or valid (WPF Property)
        public static readonly DependencyProperty DefaultBorderColorProperty = DependencyProperty.Register(
           "DefaultBorderColor",
           typeof(Brush),
           typeof(NameControl),
           new PropertyMetadata(Brushes.Gray));

        public Brush DefaultBorderColor
        {
            get { return (Brush)GetValue(DefaultBorderColorProperty); }
            set { SetValue(DefaultBorderColorProperty, value); }
        }

        // DependencyProperty to modify the warning text color when empty or valid (WPF Property)
        public static readonly DependencyProperty DefaultTextColorProperty = DependencyProperty.Register(
            "DefaultTextColor",
            typeof(Brush),
            typeof(NameControl),
            new PropertyMetadata(Brushes.Gray));

        public Brush DefaultTextColor
        {
            get { return (Brush)GetValue(DefaultTextColorProperty); }
            set { SetValue(DefaultTextColorProperty, value); }
        }

        // Background color of the TextBox
        public static readonly DependencyProperty TextBackgroundProperty = DependencyProperty.Register(
            "TextBackground", typeof(Brush), typeof(NameControl),
            new PropertyMetadata(Brushes.White));
        public Brush TextBackground
        {
            get { return (Brush)GetValue(TextBackgroundProperty); }
            set { SetValue(TextBackgroundProperty, value); }
        }

        // Foreground color of the text typed by the user
        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register(
            "TextForeground", typeof(Brush), typeof(NameControl),
            new PropertyMetadata(Brushes.Black));
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        // Internal padding of the TextBox
        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register(
            "TextPadding", typeof(Thickness), typeof(NameControl),
            new PropertyMetadata(new Thickness(4)));
        public Thickness TextPadding
        {
            get { return (Thickness)GetValue(TextPaddingProperty); }
            set { SetValue(TextPaddingProperty, value); }
        }

        // Font family
        public static readonly DependencyProperty TextFontFamilyProperty = DependencyProperty.Register(
            "TextFontFamily", typeof(FontFamily), typeof(NameControl),
            new PropertyMetadata(new FontFamily("Segoe UI")));
        public FontFamily TextFontFamily
        {
            get { return (FontFamily)GetValue(TextFontFamilyProperty); }
            set { SetValue(TextFontFamilyProperty, value); }
        }

        // Font size
        public static readonly DependencyProperty TextFontSizeProperty = DependencyProperty.Register(
            "TextFontSize", typeof(double), typeof(NameControl),
            new PropertyMetadata(13.0));
        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }

        // Caret brush color
        public static readonly DependencyProperty TextCaretBrushProperty = DependencyProperty.Register(
            "TextCaretBrush", typeof(Brush), typeof(NameControl),
            new PropertyMetadata(Brushes.Black));
        public Brush TextCaretBrush
        {
            get { return (Brush)GetValue(TextCaretBrushProperty); }
            set { SetValue(TextCaretBrushProperty, value); }
        }

        // Text selection highlight color
        public static readonly DependencyProperty TextSelectionBrushProperty = DependencyProperty.Register(
            "TextSelectionBrush", typeof(Brush), typeof(NameControl),
            new PropertyMetadata(Brushes.LightBlue));
        public Brush TextSelectionBrush
        {
            get { return (Brush)GetValue(TextSelectionBrushProperty); }
            set { SetValue(TextSelectionBrushProperty, value); }
        }

        private void NameControl_PreviewTextChange(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            string currentText = textBox?.Text;

            if (string.IsNullOrEmpty(currentText?.Trim()))
            {
                textBoxBorder.BorderBrush = DefaultBorderColor;
                warningText.Text = "";
                warningText.Foreground = DefaultTextColor;
                IsValid = false;
                return;
            }

            if (IsTextValid(currentText) && currentText.Any(char.IsDigit))
            {
                textBoxBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                warningText.Text = "Digits not allowed";
                warningText.Foreground = Brushes.Orange;
                IsValid = false;
            }
            else if (IsTextValid(currentText))
            {
                textBoxBorder.BorderBrush = DefaultBorderColor;
                warningText.Text = "";
                warningText.Foreground = DefaultTextColor;
                IsValid = true;
            }
            else
            {
                textBoxBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                warningText.Text = "At least 3 characters";
                warningText.Foreground = Brushes.Red;
                IsValid = false;
            }
        }

        // Validates that the text meets the minimum length requirement
        private bool IsTextValid(string text)
        {
            if (text.Replace(" ", "").Length < MinLength) return false;
            return true;
        }
    }
}