using System.Windows;
using System.Windows.Input;

namespace ValuteConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel(new SoapRequest());
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
         e.Command == ApplicationCommands.Cut ||
         e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
