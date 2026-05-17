namespace Model
{
    public class Ball
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public double Radius { get; set; } = 10;

        public double Mass { get; set; } = 1;

        public double Diameter => Radius * 2;
    }
}