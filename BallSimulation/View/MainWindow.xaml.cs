using Data;
using Logic;
using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var repo = new BallRepository();
            var logic = new BallLogic(repo);

            DataContext = new MainViewModel(logic);
        }
    }
}