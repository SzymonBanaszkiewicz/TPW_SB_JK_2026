using Model;

namespace Logic
{

    public interface IBallLogic
    {
        void CreateBalls(int count);
        IEnumerable<Ball> GetBalls();
        void Update(double width, double height);
    }
}
