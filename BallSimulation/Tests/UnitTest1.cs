using Logic;
using Model;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Ball_Should_Move()
        {
            var ball = new Ball { X = 0, Y = 0 };
            var logic = new BallLogic();

            logic.Move(ball);

            Assert.Equal(1, ball.X);
            Assert.Equal(1, ball.Y);
        }
    }
}
