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
            foreach (var ball in repository.GetBalls())
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;

                // odbicie od ścian
                if (ball.X <= 0 || ball.X + ball.Radius * 2 >= width)
                    ball.VelocityX *= -1;

                if (ball.Y <= 0 || ball.Y + ball.Radius * 2 >= height)
                    ball.VelocityY *= -1;
            }
        }
    }
}
