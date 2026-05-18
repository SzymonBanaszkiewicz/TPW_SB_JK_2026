using Data;
using Model;

namespace Tests
{
    public class FakeBallRepository : IBallRepository
    {
        private readonly List<Ball> balls;

        public FakeBallRepository(List<Ball> balls)
        {
            this.balls = balls;
        }

        public IEnumerable<Ball> GetBalls()
        {
            return balls;
        }

        public void CreateBalls(int count, double width, double height)
        {
        }

        public void MoveBalls()
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;
            }
        }
    }
}
