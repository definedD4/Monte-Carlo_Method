using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;
using System.Threading;

namespace Monte_Carlo_Method_3D.Calculation
{
    public class PrCalculation : Calculation
    {
        private readonly int m_Width;
        private readonly int m_Height;
        private readonly GridData m_EdgeData;

        public PrCalculation(ICalculationConstraint constraint, int width, int height, GridData edgeData) : base(constraint)
        {
            m_Width = width;
            m_Height = height;
            m_EdgeData = edgeData;
        }

        protected override GridData Simulate()
        {
            for(int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                ReportProgress(i * 10);
            }
            return null;
        }
    }
}
