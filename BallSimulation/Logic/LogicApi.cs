using System;
using System.Collections.Generic;
using Data;

namespace Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;
        private readonly object _collisionLock = new();

        public override double Width { get; set; } = 500;
        public override double Height { get; set; } = 300;

        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }

        public override void StartSimulation(int ballCount)
        {
            _dataApi.CreateBalls(ballCount, Width, Height);

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

        private void OnBallPositionChanged(object? sender, IBall ball)
        {
            HandleWallCollision(ball);
            HandleBallCollisions(ball);
        }

        private void HandleWallCollision(IBall ball)
        {
            if (ball.X <= 0 && ball.VelocityX < 0)
            {
                ball.VelocityX = -ball.VelocityX;
            }
            else if (ball.X + ball.Diameter >= Width && ball.VelocityX > 0)
            {
                ball.VelocityX = -ball.VelocityX;
            }

            if (ball.Y <= 0 && ball.VelocityY < 0)
            {
                ball.VelocityY = -ball.VelocityY;
            }
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

            double restitution = 1.0;
            double impulse = -((1.0 + restitution) * velocityAlongNormal) / (1.0 / b1.Mass + 1.0 / b2.Mass);

            b1.VelocityX -= (impulse / b1.Mass) * nx;
            b1.VelocityY -= (impulse / b1.Mass) * ny;
            b2.VelocityX += (impulse / b2.Mass) * nx;
            b2.VelocityY += (impulse / b2.Mass) * ny;

            double overlap = (b1.Radius + b2.Radius) - distance;
            if (overlap > 0)
            {
                double totalMass = b1.Mass + b2.Mass;
                double separationX = nx * overlap;
                double separationY = ny * overlap;

                b1.SetPosition(b1.X - separationX * (b2.Mass / totalMass), b1.Y - separationY * (b2.Mass / totalMass));
                b2.SetPosition(b2.X + separationX * (b1.Mass / totalMass), b2.Y + separationY * (b1.Mass / totalMass));
            }
        }
    }
}