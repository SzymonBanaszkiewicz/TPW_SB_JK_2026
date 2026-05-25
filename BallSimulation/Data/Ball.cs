using System;
using System.Threading.Tasks;

namespace Data
{
    internal class Ball : IBall, IDisposable
    {
        private double _x;
        private double _y;
        private double _velocityX;
        private double _velocityY;
        private bool _isRunning = true;

        public override event EventHandler<IBall>? PositionChanged;

        public override double X => _x;
        public override double Y => _y;

        public override double VelocityX
        {
            get => _velocityX;
            set => _velocityX = value;
        }

        public override double VelocityY
        {
            get => _velocityY;
            set => _velocityY = value;
        }

        public override double Radius { get; }
        public override double Mass { get; }
        public override double Diameter => Radius * 2;

        public Ball(double x, double y, double vx, double vy, double radius, double mass)
        {
            _x = x;
            _y = y;
            _velocityX = vx;
            _velocityY = vy;
            Radius = radius;
            Mass = mass;

            Task.Run(BallLoop);
        }

        private async Task BallLoop()
        {
            while (_isRunning)
            {
                Move();
                PositionChanged?.Invoke(this, this);

                await Task.Delay(16);
            }
        }

        public override void Move()
        {
            _x += _velocityX;
            _y += _velocityY;
        }

        public override void SetPosition(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public void Dispose()
        {
            _isRunning = false;
        }
    }
}