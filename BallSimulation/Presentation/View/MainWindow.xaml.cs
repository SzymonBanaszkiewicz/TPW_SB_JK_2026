using System.Windows;
using Presentation.ViewModel;

namespace Presentation.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                double newWidth = e.NewSize.Width;
                double newHeight = e.NewSize.Height;

                viewModel.ChangeStageSize(newWidth, newHeight);
            }
        }
    }
}