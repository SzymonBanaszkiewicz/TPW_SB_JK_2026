using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Data;

namespace Presentation.ViewModel
{
    public class BallViewModel : INotifyPropertyChanged
    {
        private readonly IBall _ball;

        // Właściwości bindowane bezpośrednio w XAML (np. do Canvas.Left i Canvas.Top)
        public double X => _ball.X;
        public double Y => _ball.Y;
        public double Diameter => _ball.Diameter;
        public double Radius => _ball.Radius;

        public BallViewModel(IBall ball)
        {
            _ball = ball;
            // Reaktywne podpięcie pod zdarzenie zmiany pozycji z warstwy danych
            _ball.PositionChanged += OnBallPositionChanged;
        }

        private void OnBallPositionChanged(object? sender, IBall e)
        {
            // Ponieważ każda kula działa w osobnym wątku (Task), aktualizacja UI 
            // musi zostać przekierowana bezpiecznie do wątku głównego aplikacji WPF.
            Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
            {
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
            }));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}