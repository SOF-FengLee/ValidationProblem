using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace TestValidation.UserControls
{
    /// <summary>
    /// Interaction logic for TextBoxWithButton.xaml
    /// </summary>
    public partial class TextBoxWithButton : UserControl, INotifyPropertyChanged
    {
        private string _placeholder = "Placeholder...";

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
                                                                                             typeof(string),
                                                                                             typeof(TextBoxWithButton),
                                                                                             new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(ButtonCommand),
                                                                                                typeof(ICommand),
                                                                                                typeof(TextBoxWithButton),
                                                                                                new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(ButtonCommandParameter),
                                                                                                         typeof(object),
                                                                                                         typeof(TextBoxWithButton),
                                                                                                         new PropertyMetadata(null));

        public TextBoxWithButton()
        {
            InitializeComponent();
        }

        public string Placeholder 
        { 
            get => _placeholder; 
            set 
            {
                _placeholder = value;
                OnPropertyChanged();
            } 
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                OnPropertyChanged();
            }
        }

        public ICommand? ButtonCommand
        {
            get => (ICommand)GetValue(CommandProperty);
            set
            {
                SetValue(CommandProperty, value);
                OnPropertyChanged();
            }
        }

        public object? ButtonCommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set
            {
                SetValue(CommandParameterProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text))
                tbPlaceholder.Visibility = Visibility.Visible;
            else
                tbPlaceholder.Visibility = Visibility.Hidden;

        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BindingExpression? binding = txtInput.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
                e.Handled = true;
            }
        }
    }
}
