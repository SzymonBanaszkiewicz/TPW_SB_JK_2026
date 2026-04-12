using System.ComponentModel;

namespace ViewModel
{
    public class BallViewModel : INotifyPropertyChanged
    {
        private double x;
        private double y;

        public double X
        {
            get => x;
            set { x = value; OnPropertyChanged(nameof(X)); }
        }

        public double Y
        {
            get => y;
            set { y = value; OnPropertyChanged(nameof(Y)); }
        }

        public double Diameter { get; set; } = 20;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
