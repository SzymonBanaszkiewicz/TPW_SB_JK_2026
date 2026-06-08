using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    internal class DataApi : DataAbstractApi
    {
        private readonly List<Ball> _balls = new();
        private readonly Random _random = new();
        private readonly object _ballsLock = new();

        public override void CreateBalls(int count, double width, double height)
        {
            lock (_ballsLock)
            {
                Stop();
                _balls.Clear();

                for (int i = 0; i < count; i++)
                {
                    double radius = _random.Next(10, 20);
                    double mass = radius;

                    double x = _random.NextDouble() * (width - (2 * radius));
                    double y = _random.NextDouble() * (height - (2 * radius));

                    double vx = (_random.NextDouble() * 3 + 1) * (_random.Next(2) == 0 ? 1 : -1);
                    double vy = (_random.NextDouble() * 3 + 1) * (_random.Next(2) == 0 ? 1 : -1);

                    Ball newBall = new Ball(i, x, y, vx, vy, radius, mass);
                    _balls.Add(newBall);
                }
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            lock (_ballsLock)
            {
                return _balls.Cast<IBall>().ToList();
            }
        }

        public override void Stop()
        {
            lock (_ballsLock)
            {
                foreach (var ball in _balls)
                {
                    ball.Dispose();
                }
                DiagnosticLogger.Stop();
            }
        }
    }
}