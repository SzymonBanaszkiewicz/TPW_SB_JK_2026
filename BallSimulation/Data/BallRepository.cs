using Model;

namespace Data
{
    public class BallRepository : IBallRepository
    {
        private readonly List<Ball> balls = new();
        private readonly Random random = new();

        public IEnumerable<Ball> GetBalls() => balls;

        public void CreateBalls(int count, double width, double height)
        {
            balls.Clear();
            
            for (int i = 0; i < count; i++)
            {
                balls.Add(new Ball
                {
                    X = random.NextDouble() * width/1.1,
                    Y = random.NextDouble() * height/1.1,
                    VelocityX = random.NextDouble() * 4 - 2,
                    VelocityY = random.NextDouble() * 4 - 2
                });
            }
        }
    }
}
