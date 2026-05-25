using Xunit;
using System.Linq;
using Data;

namespace ProjectTests
{
    public class DataTests
    {
        [Fact]
        public void CreateBalls_ShouldCreateCorrectNumberOfBalls()
        {
            DataAbstractApi dataApi = DataAbstractApi.CreateApi();
            int expectedBallCount = 5;

            dataApi.CreateBalls(expectedBallCount, 500, 300);
            var balls = dataApi.GetBalls().ToList();

            Assert.Equal(expectedBallCount, balls.Count);

            dataApi.Stop();
        }
    }
}