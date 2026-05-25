using System;
using System.Collections.Generic;
using Data;

namespace ProjectTests
{
    public class FakeBall : IBall
    {
        private EventHandler<IBall>? _positionChanged;
        public override event EventHandler<IBall>? PositionChanged
        {
            add => _positionChanged += value;
            remove => _positionChanged -= value;
        }

        public override double X { get; }
        public override double Y { get; }

        private double _velocityX;
        public override double VelocityX
        {
            get => _velocityX;
            set => _velocityX = value;
        }

        private double _velocityY;
        public override double VelocityY
        {
            get => _velocityY;
            set => _velocityY = value;
        }

        public override double Radius { get; }
        public override double Mass { get; }
        public override double Diameter => Radius * 2;

        public FakeBall(double x, double y, double vx, double vy, double radius, double mass)
        {
            X = x;
            Y = y;
            _velocityX = vx;
            _velocityY = vy;
            Radius = radius;
            Mass = mass;
        }

        public override void Move()
        {
            _positionChanged?.Invoke(this, this);
        }
    }

    public class FakeDataApi : DataAbstractApi
    {
        public List<IBall> Balls { get; set; } = new List<IBall>();

        public override void CreateBalls(int count, double width, double height)
        {
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return Balls;
        }

        public override void Stop()
        {
        }
    }
}