using System.Collections.Generic;
using Data;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }

        public abstract void StartSimulation(int ballCount);
        public abstract IEnumerable<IBall> GetBalls();
        public abstract void StopSimulation();

        public static LogicAbstractApi CreateApi(DataAbstractApi? dataApi = null)
        {
            return new LogicApi(dataApi ?? DataAbstractApi.CreateApi());
        }
    }
}