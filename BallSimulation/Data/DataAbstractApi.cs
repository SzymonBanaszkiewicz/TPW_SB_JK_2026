using System.Collections.Generic;

namespace Data
{
    public abstract class DataAbstractApi
    {
        public abstract void CreateBalls(int count, double width, double height);
        public abstract IEnumerable<IBall> GetBalls();
        public abstract void Stop();
        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
}