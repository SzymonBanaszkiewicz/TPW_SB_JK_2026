using Xunit;
using Logic;

namespace ProjectTests
{
    public class LogicTests
    {
        [Fact]
        public void WallCollision_ShouldInvertVelocity_WhenBallHitsLeftWall()
        {
            var fakeDataApi = new FakeDataApi();

            var ballNearWall = new FakeBall(0, 50, -2, 0, 10, 10);
            fakeDataApi.Balls.Add(ballNearWall);

            LogicAbstractApi logicApi = LogicAbstractApi.CreateApi(fakeDataApi);

            logicApi.StartSimulation(1);

            ballNearWall.Move();

            Assert.Equal(2, ballNearWall.VelocityX);

            logicApi.StopSimulation();
        }
    }
}