using Model;

namespace Data
{
    public class BallRepository : IBallRepository
    {
        private readonly List<Ball> balls = new();
        private readonly Random random = new();
        private readonly object _lock = new();

        public IEnumerable<Ball> GetBalls()
        {
            lock (_lock)
            {
                return balls.ToList();
            }
        }

        public void CreateBalls(int count, double width, double height)
        {
            lock (_lock)
            {
                balls.Clear();

                for (int i = 0; i < count; i++)
                {
                    double radius = random.Next(10, 25);

                    balls.Add(new Ball
                    {
                        Radius = radius,
                        Mass = radius, 

                        X = random.NextDouble() * (width - radius * 2),
                        Y = random.NextDouble() * (height - radius * 2),

                        VelocityX = random.NextDouble() * 4 - 2,
                        VelocityY = random.NextDouble() * 4 - 2
                    });
                }
            }
        }

        public void MoveBalls()
        {
            lock (_lock)
            {
                foreach (var ball in balls)
                {
                    ball.X += ball.VelocityX;
                    ball.Y += ball.VelocityY;
                }
            }
        }
    }
}
