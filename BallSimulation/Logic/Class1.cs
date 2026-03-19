using Model;

namespace Logic
{
    public class BallLogic
    {
        public void Move(Ball ball)
        {
            ball.X += 1;
            ball.Y += 1;
        }
    }
}
