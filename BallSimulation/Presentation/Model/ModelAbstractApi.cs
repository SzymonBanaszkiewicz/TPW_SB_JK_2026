using System.Collections.Generic;
using Data;
using Logic;

namespace Presentation.Model
{
    public abstract class ModelAbstractApi
    {
        public abstract void Start(int ballCount);
        public abstract void Stop();
        public abstract IEnumerable<IBall> GetBalls();

        public static ModelAbstractApi CreateApi(LogicAbstractApi? logicApi = null)
        {
            return new ModelApi(logicApi ?? LogicAbstractApi.CreateApi());
        }
    }
}