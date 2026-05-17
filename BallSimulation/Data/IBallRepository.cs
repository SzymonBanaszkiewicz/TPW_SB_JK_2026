using Model;

namespace Data
{
    public interface IBallRepository
    {
        IEnumerable<Ball> GetBalls();
        void CreateBalls(int count, double width, double height);

        void MoveBalls();
    }
}
