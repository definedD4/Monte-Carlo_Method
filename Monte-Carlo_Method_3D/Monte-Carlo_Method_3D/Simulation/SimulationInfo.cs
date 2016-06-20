using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class SimulationInfo
    {
        public SimulationInfo(int step, int totalSimTime, double centerSum, double edgeSum, double totalSum)
        {
            Step = step;
            TotalSimTime = totalSimTime;
            CenterSum = centerSum;
            EdgeSum = edgeSum;
            TotalSum = totalSum;
        }

        public int Step { get; private set; }
        public int TotalSimTime { get; private set; }
        public double CenterSum { get; private set; }
        public double EdgeSum { get; private set; }
        public double TotalSum { get; private set; }
    }
}
