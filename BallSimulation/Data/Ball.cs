using System;
using System.Threading;

namespace Data
{
    internal class Ball : IBall, IDisposable
    {
        private double _x;
        private double _y;
        private double _velocityX;
        private double _velocityY;

        private readonly object _ballLock = new();
        private readonly Timer _timer;

        private bool _disposed;
        private int _isUpdating; 

        private readonly int _id;
        private const double DeltaTime = 0.016; // 16 ms

        public override event EventHandler<IBall>? PositionChanged;

        public override double X { get { lock (_ballLock) return _x; } }
        public override double Y { get { lock (_ballLock) return _y; } }
        public override double VelocityX
        {
            get { lock (_ballLock) return _velocityX; }
            set { lock (_ballLock) _velocityX = value; }
        }
        public override double VelocityY
        {
            get { lock (_ballLock) return _velocityY; }
            set { lock (_ballLock) _velocityY = value; }
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

            _timer = new Timer(Update, null, 0, 16);
        }

        private void Update(object? state)
        {
            if (_disposed) return;

            if (Interlocked.CompareExchange(ref _isUpdating, 1, 0) != 0)
                return;

            try
            {
                double localX, localY, localVx, localVy;

                lock (_ballLock)
                {
                    _x += _velocityX * DeltaTime;
                    _y += _velocityY * DeltaTime;

                    localX = _x;
                    localY = _y;
                    localVx = _velocityX;
                    localVy = _velocityY;
                }


                PositionChanged?.Invoke(this, this);

                DiagnosticLogger.QueueLog(
                    $"{DateTime.Now:O} | Ball: {_id} | Pos: ({localX:F2}, {localY:F2}) | Vel: ({localVx:F2}, {localVy:F2})");
            }
            finally
            {
                Interlocked.Exchange(ref _isUpdating, 0);
            }
        }

        public override void SetPosition(double x, double y)
        {
            lock (_ballLock)
            {
                _x = x;
                _y = y;
            }
        }

        public override void Move()
        {
            lock (_ballLock)
            {
                _x += _velocityX * DeltaTime;
                _y += _velocityY * DeltaTime;
            }
        }

        public void Dispose()
        {
            _disposed = true;
            _timer.Dispose();
        }
    }
}