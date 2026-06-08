using System;
using System.Diagnostics;
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
        private readonly object _ballLock = new();

        private readonly int _id; 

        public override event EventHandler<IBall>? PositionChanged;

        public override double X { get { lock (_ballLock) return _x; } }
        public override double Y { get { lock (_ballLock) return _y; } }

        public override double VelocityX
        {
            get { lock (_ballLock) return _velocityX; }
            set { lock (_ballLock) { _velocityX = value; } }
        }

        public override double VelocityY
        {
            get { lock (_ballLock) return _velocityY; }
            set { lock (_ballLock) { _velocityY = value; } }
        }

        public override double Radius { get; }
        public override double Mass { get; }
        public override double Diameter => Radius * 2;

        public Ball(int id, double x, double y, double vx, double vy, double radius, double mass)
        {
            _id = id;
            _x = x;
            _y = y;
            _velocityX = vx * 100; 
            _velocityY = vy * 100;
            Radius = radius;
            Mass = mass;

            Task.Run(BallLoop);
        }

        private async Task BallLoop()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_isRunning)
            {
                long elapsedMs = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();

                double deltaTime = elapsedMs / 1000.0;

                Move(deltaTime);
                PositionChanged?.Invoke(this, this);

                string logData = $"{DateTime.Now:O} | Ball: {_id} | Pos: ({X:F2}, {Y:F2}) | Vel: ({VelocityX:F2}, {VelocityY:F2})";
                DiagnosticLogger.QueueLog(logData);

                await Task.Delay(16); 
            }
        }

        public void Move(double deltaTime)
        {
            lock (_ballLock)
            {
                _x += _velocityX * deltaTime;
                _y += _velocityY * deltaTime;
            }
        }

        public override void Move()
        {
            Move(0.016);
        }

        public override void SetPosition(double x, double y)
        {
            lock (_ballLock)
            {
                _x = x;
                _y = y;
            }
        }

        public void Dispose()
        {
            _isRunning = false;
        }
    }
}