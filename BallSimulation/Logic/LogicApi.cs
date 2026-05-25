using System;
using System.Collections.Generic;
using Data;

namespace Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;
        private readonly object _collisionLock = new(); // Sekcja krytyczna 

        private const double Width = 500;
        private const double Height = 300;

        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }

        public override void StartSimulation(int ballCount)
        {
            _dataApi.CreateBalls(ballCount, Width, Height);

            // Reaktywnosc
            foreach (var ball in _dataApi.GetBalls())
            {
                ball.PositionChanged += OnBallPositionChanged;
            }
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _dataApi.GetBalls();
        }

        public override void StopSimulation()
        {
            foreach (var ball in _dataApi.GetBalls())
            {
                ball.PositionChanged -= OnBallPositionChanged;
            }
            _dataApi.Stop();
        }

        // wywolanie asynchronicznie
        private void OnBallPositionChanged(object? sender, IBall ball)
        {
            // Ochrona 
            lock (_collisionLock)
            {
                HandleWallCollision(ball);
                HandleBallCollisions(ball);
            }
        }

        private void HandleWallCollision(IBall ball)
        {
            // lewa
            if (ball.X <= 0 && ball.VelocityX < 0)
            {
                ball.VelocityX = -ball.VelocityX;
            }
            // prawa
            else if (ball.X + ball.Diameter >= Width && ball.VelocityX > 0)
            {
                ball.VelocityX = -ball.VelocityX;
            }

            // górna
            if (ball.Y <= 0 && ball.VelocityY < 0)
            {
                ball.VelocityY = -ball.VelocityY;
            }
            // dolna
            else if (ball.Y + ball.Diameter >= Height && ball.VelocityY > 0)
            {
                ball.VelocityY = -ball.VelocityY;
            }
        }

        private void HandleBallCollisions(IBall currentBall)
        {
            foreach (var otherBall in _dataApi.GetBalls())
            {
                if (currentBall == otherBall) continue;

                double dx = (otherBall.X + otherBall.Radius) - (currentBall.X + currentBall.Radius);
                double dy = (otherBall.Y + otherBall.Radius) - (currentBall.Y + currentBall.Radius);
                double distance = Math.Sqrt(dx * dx + dy * dy);
                double minDistance = currentBall.Radius + otherBall.Radius;

                if (distance < minDistance)
                {
                    ResolveCollision(currentBall, otherBall, distance, dx, dy);
                }
            }
        }

        private void ResolveCollision(IBall b1, IBall b2, double distance, double dx, double dy)
        {
            if (distance == 0) return;

            double nx = dx / distance;
            double ny = dy / distance;

            double rvx = b2.VelocityX - b1.VelocityX;
            double rvy = b2.VelocityY - b1.VelocityY;

            double velocityAlongNormal = rvx * nx + rvy * ny;

            if (velocityAlongNormal > 0) return;

            double impulse = -(2.0 * velocityAlongNormal) / (1.0 / b1.Mass + 1.0 / b2.Mass);

            b1.VelocityX -= (impulse / b1.Mass) * nx;
            b1.VelocityY -= (impulse / b1.Mass) * ny;
            b2.VelocityX += (impulse / b2.Mass) * nx;
            b2.VelocityY += (impulse / b2.Mass) * ny;
        }
    }
}