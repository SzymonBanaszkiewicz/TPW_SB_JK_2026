using Data;
using Logic;
using Model;

namespace Tests
{
    public class BallLogicTests
    {
        [Fact]
        public void Ball_ShouldBounceFromWall()
        {
            var balls = new List<Ball>
            {
                new Ball
                {
                    X = 0,
                    Y = 100,
                    VelocityX = -2,
                    VelocityY = 0,
                    Radius = 10,
                    Mass = 1
                }
            };

            var repository = new FakeBallRepository(balls);

            var logic = new BallLogic(repository);

            logic.Update(500, 300);

            Assert.True(balls[0].VelocityX > 0);
        }

        [Fact]
        public void Balls_ShouldChangeVelocityAfterCollision()
        {
            var balls = new List<Ball>
            {
                new Ball
                {
                    X = 100,
                    Y = 100,
                    VelocityX = 1,
                    VelocityY = 0,
                    Radius = 10,
                    Mass = 1
                },

                new Ball
                {
                    X = 119,
                    Y = 100,
                    VelocityX = -1,
                    VelocityY = 0,
                    Radius = 10,
                    Mass = 1
                }
            };

            var repository = new FakeBallRepository(balls);

            var logic = new BallLogic(repository);

            logic.Update(500, 300);

            Assert.True(balls[0].VelocityX < 0);
            Assert.True(balls[1].VelocityX > 0);
        }
    }
}