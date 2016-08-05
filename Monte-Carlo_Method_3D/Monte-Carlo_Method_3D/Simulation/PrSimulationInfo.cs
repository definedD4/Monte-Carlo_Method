using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class PrSimulationInfo
    {
        public PrSimulationInfo(long step, double totalSimTime, double centerSum, double edgeSum, double totalSum)
        {
            Step = step;
            TotalSimTime = totalSimTime;
            CenterSum = centerSum;
            EdgeSum = edgeSum;
            TotalSum = totalSum;
        }

        public long Step { get; private set; }
        public double TotalSimTime { get; private set; }
        public double CenterSum { get; private set; }
        public double EdgeSum { get; private set; }
        public double TotalSum { get; private set; }
    }
}
