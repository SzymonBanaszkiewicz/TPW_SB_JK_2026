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

        private readonly object _positionLock = new();
        private readonly object _velocityLock = new();
        private bool _isRunning = true;

        public override event EventHandler<IBall>? PositionChanged;

        public override double X
        {
            get { lock (_positionLock) return _x; }
        }

        public override double Y
        {
            get { lock (_positionLock) return _y; }
        }

        public override double VelocityX
        {
            get { lock (_velocityLock) return _velocityX; }
            set { lock (_velocityLock) _velocityX = value; }
        }

        public override double VelocityY
        {
            get { lock (_velocityLock) return _velocityY; }
            set { lock (_velocityLock) _velocityY = value; }
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

                await Task.Delay(16);
            }
        }

        public override void Move()
        {
            lock (_positionLock)
            {
                _x += VelocityX;
                _y += VelocityY;
            }

            PositionChanged?.Invoke(this, this);
        }

        public void Dispose()
        {
            _isRunning = false;
        }
    }
}