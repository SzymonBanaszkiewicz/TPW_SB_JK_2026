using Presentation.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Presentation.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ModelAbstractApi _modelApi;
        private int _ballCount = 5; // Domyślna wartość początkowa kul

        public ObservableCollection<BallViewModel> Balls { get; } = new();
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public int BallCount
        {
            get => _ballCount;
            set
            {
                if (_ballCount == value) return;
                _ballCount = value;
                OnPropertyChanged();
            }
        }

        // Konstruktor bezparametrowy wymagany do poprawnego bindowania w XAML (jako DataContext)
        public MainViewModel() : this(ModelAbstractApi.CreateApi()) { }

        // Konstruktor z wstrzykiwaniem zależności (Dependency Injection) ułatwiający testowanie
        public MainViewModel(ModelAbstractApi modelApi)
        {
            _modelApi = modelApi;
            StartCommand = new RelayCommand(StartSimulation);
            StopCommand = new RelayCommand(StopSimulation);
        }

        private void StartSimulation()
        {
            StopSimulation(); // Zapewnia czyszczenie przed nowym uruchomieniem
            _modelApi.Start(BallCount);

            foreach (var ball in _modelApi.GetBalls())
            {
                Balls.Add(new BallViewModel(ball));
            }
        }

        private void StopSimulation()
        {
            _modelApi.Stop();
            Balls.Clear();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}