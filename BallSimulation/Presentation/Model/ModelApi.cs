using System.Collections.Generic;
using Data;
using Logic;

namespace Presentation.Model
{
    internal class ModelApi : ModelAbstractApi
    {
        private readonly LogicAbstractApi _logicApi;

        public ModelApi(LogicAbstractApi logicApi)
        {
            _logicApi = logicApi;
        }

        public override void Start(int ballCount)
        {
            _logicApi.StartSimulation(ballCount);
        }

        public override void Stop()
        {
            _logicApi.StopSimulation();
        }

        public override void UpdateBoardSize(double width, double height)
        {
            _logicApi.Width = width;
            _logicApi.Height = height;
        }

        public override IEnumerable<IBall> GetBalls()
        {
            return _logicApi.GetBalls();
        }
    }
}