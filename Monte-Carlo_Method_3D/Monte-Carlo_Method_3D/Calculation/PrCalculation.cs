using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Calculation
{
    public class PrCalculation : Calculation
    {
        private PrSimulator m_Simulator;

        public PrCalculation(ICalculationConstraint constraint, SimulationOptions options) : base(constraint)
        {
            m_Simulator = new PrSimulator(options);
        }

        protected override double[,] Simulate()
        {
            throw new NotImplementedException();
        }
    }
}
