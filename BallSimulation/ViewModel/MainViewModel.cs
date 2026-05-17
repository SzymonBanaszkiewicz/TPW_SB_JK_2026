using Logic;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;

namespace ViewModel
{
    public class MainViewModel
    {
        private readonly IBallLogic logic;

        private readonly CancellationTokenSource tokenSource = new();

        private const double CanvasWidth = 500;
        private const double CanvasHeight = 300;

        public ObservableCollection<BallViewModel> Balls { get; set; } = new();

        public ICommand CreateBallsCommand { get; }

        public int BallCount { get; set; } = 5;

        public MainViewModel(IBallLogic logic)
        {
            this.logic = logic;

            CreateBallsCommand = new RelayCommand(CreateBalls);

            StartSimulation();
        }

        private void CreateBalls()
        {
            logic.CreateBalls(BallCount);

            Balls.Clear();

            foreach (var ball in logic.GetBalls())
            {
                Balls.Add(new BallViewModel
                {
                    X = ball.X,
                    Y = ball.Y,
                    Diameter = ball.Diameter
                });
            }
        }

        private async void StartSimulation()
        {
            while (!tokenSource.Token.IsCancellationRequested)
            {
                logic.Update(CanvasWidth, CanvasHeight);

                var logicBalls = logic.GetBalls().ToList();

                if (logicBalls.Count == Balls.Count)
                {
                    for (int i = 0; i < logicBalls.Count; i++)
                    {
                        Balls[i].X = logicBalls[i].X;
                        Balls[i].Y = logicBalls[i].Y;
                    }
                }

                await Task.Delay(20);
            }
        }
    }
}