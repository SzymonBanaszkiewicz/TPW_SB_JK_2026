using System;

namespace Data
{
    public interface INotifyPositionChanged
    {
        event EventHandler<IBall>? PositionChanged;
    }

    public abstract class IBall : INotifyPositionChanged
    {
        public abstract event EventHandler<IBall>? PositionChanged;

        public abstract double X { get; }
        public abstract double Y { get; }
        public abstract double VelocityX { get; set; }
        public abstract double VelocityY { get; set; }
        public abstract double Radius { get; }
        public abstract double Mass { get; }
        public abstract double Diameter { get; }

        public abstract void Move();
        public abstract void SetPosition(double x, double y);
    }
}