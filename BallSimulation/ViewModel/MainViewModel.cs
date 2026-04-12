using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

using System.Windows.Threading;

namespace ViewModel
{
    public class MainViewModel
    {
        private readonly IBallLogic logic;
        private readonly DispatcherTimer timer;

        public ObservableCollection<BallViewModel> Balls { get; set; } = new();

        public ICommand CreateBallsCommand { get; }

        public int BallCount { get; set; } = 5;

        public MainViewModel(IBallLogic logic)
        {
            this.logic = logic;

            CreateBallsCommand = new RelayCommand(CreateBalls);

            timer = new DispatcherTimer();
            timer.Interval = System.TimeSpan.FromMilliseconds(20);
            timer.Tick += (s, e) => Update();
            timer.Start();
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
                    Y = ball.Y
                });
            }
        }

        private void Update()
        {
            logic.Update(500, 300);

            int i = 0;
            foreach (var ball in logic.GetBalls())
            {
                Balls[i].X = ball.X;
                Balls[i].Y = ball.Y;
                i++;
            }
        }
    }
}
