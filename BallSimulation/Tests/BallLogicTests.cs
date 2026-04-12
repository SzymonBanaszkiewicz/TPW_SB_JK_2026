using Data;
using Logic;
using Model;

namespace Tests
{
    public class BallLogicTests
    {
        [Fact]
        public void CreateBalls_ShouldCreateCorrectNumber()
        {
            var repo = new BallRepository();
            var logic = new BallLogic(repo);

            logic.CreateBalls(5);

            Assert.Equal(5, logic.GetBalls().Count());
        }

        [Fact]
        public void Update_ShouldChangePosition()
        {
            var repo = new BallRepository();
            var logic = new BallLogic(repo);

            logic.CreateBalls(1);
            var ball = logic.GetBalls().First();

            var oldX = ball.X;

            logic.Update(500, 300);

            Assert.NotEqual(oldX, ball.X);
        }
    }
}
