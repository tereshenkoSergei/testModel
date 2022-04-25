using System.Text.RegularExpressions;
using System.Windows.Input;
using TestModel.Code;

namespace TestModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }   
        private void DoubleNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            Regex regexWithPoint = new Regex("[^0-9]+.[^0-9]+");
            e.Handled = 
                (regex.IsMatch(e.Text) ||
                 regexWithPoint.IsMatch(e.Text)
                 );
        }
    }
    
}