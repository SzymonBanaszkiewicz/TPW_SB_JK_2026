using Data;
using Model;


namespace Logic
{
    public class BallLogic : IBallLogic
    {
        private readonly IBallRepository repository;

        public BallLogic(IBallRepository repository)
        {
            this.repository = repository;
        }

        public void CreateBalls(int count)
        {
            repository.CreateBalls(count, 500, 300);
        }

        public IEnumerable<Ball> GetBalls() => repository.GetBalls();

        public void Update(double width, double height)
        {
            repository.MoveBalls();

            var balls = repository.GetBalls().ToList();

            HandleWallCollisions(balls, width, height);
            HandleBallCollisions(balls);
        }

        private void HandleWallCollisions(List<Ball> balls, double width, double height)
        {
            foreach (var ball in balls)
            {
                if (ball.X <= 0)
                {
                    ball.X = 0;
                    ball.VelocityX *= -1;
                }

                if (ball.X + ball.Diameter >= width)
                {
                    ball.X = width - ball.Diameter;
                    ball.VelocityX *= -1;
                }

                if (ball.Y <= 0)
                {
                    ball.Y = 0;
                    ball.VelocityY *= -1;
                }

                if (ball.Y + ball.Diameter >= height)
                {
                    ball.Y = height - ball.Diameter;
                    ball.VelocityY *= -1;
                }
            }
        }

        private void HandleBallCollisions(List<Ball> balls)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                for (int j = i + 1; j < balls.Count; j++)
                {
                    var b1 = balls[i];
                    var b2 = balls[j];

                    double dx = (b2.X + b2.Radius) - (b1.X + b1.Radius);
                    double dy = (b2.Y + b2.Radius) - (b1.Y + b1.Radius);

                    double distance = Math.Sqrt(dx * dx + dy * dy);
                    double minDistance = b1.Radius + b2.Radius;

                    if (distance < minDistance)
                    {
                        ResolveCollision(b1, b2);
                    }
                }
            }
        }

        private void ResolveCollision(Ball b1, Ball b2)
        {
            double x1 = b1.X + b1.Radius;
            double y1 = b1.Y + b1.Radius;

            double x2 = b2.X + b2.Radius;
            double y2 = b2.Y + b2.Radius;

            double dx = x2 - x1;
            double dy = y2 - y1;

            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0)
                return;

            double nx = dx / distance;
            double ny = dy / distance;

            double rvx = b2.VelocityX - b1.VelocityX;
            double rvy = b2.VelocityY - b1.VelocityY;

            double velocityAlongNormal = rvx * nx + rvy * ny;

            if (velocityAlongNormal > 0)
                return;

            double restitution = 1.0;

            double impulseScalar =
                -(1 + restitution) * velocityAlongNormal;

            impulseScalar /= (1 / b1.Mass) + (1 / b2.Mass);

            double impulseX = impulseScalar * nx;
            double impulseY = impulseScalar * ny;

            b1.VelocityX -= (1 / b1.Mass) * impulseX;
            b1.VelocityY -= (1 / b1.Mass) * impulseY;

            b2.VelocityX += (1 / b2.Mass) * impulseX;
            b2.VelocityY += (1 / b2.Mass) * impulseY;

            double overlap = (b1.Radius + b2.Radius) - distance;

            if (overlap > 0)
            {
                double totalMass = b1.Mass + b2.Mass;

                double correctionX = overlap * nx;
                double correctionY = overlap * ny;

                b1.X -= correctionX * (b2.Mass / totalMass);
                b1.Y -= correctionY * (b2.Mass / totalMass);

                b2.X += correctionX * (b1.Mass / totalMass);
                b2.Y += correctionY * (b1.Mass / totalMass);
            }
        }
    }
}
