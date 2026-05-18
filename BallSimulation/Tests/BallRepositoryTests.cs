using Data;
using Logic;

namespace Tests
{
    public class BallRepositoryTests
    {
        [Fact]
        public void CreateBalls_ShouldCreateCorrectNumberOfBalls()
        {
            var repository = new BallRepository();

            repository.CreateBalls(5, 500, 300);

            Assert.Equal(5, repository.GetBalls().Count());
        }

        [Fact]
        public void MoveBalls_ShouldChangeBallPosition()
        {
            var repository = new BallRepository();

            repository.CreateBalls(1, 500, 300);

            var ball = repository.GetBalls().First();

            double oldX = ball.X;

            repository.MoveBalls();

            Assert.NotEqual(oldX, ball.X);
        }
    }
}
