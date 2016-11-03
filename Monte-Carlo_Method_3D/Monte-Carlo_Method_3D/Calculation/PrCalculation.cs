using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;
using System.Threading;
using Monte_Carlo_Method_3D.Util;

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
            double[,] res = new double[m_Width, m_Height];

            int totalCells = (m_Width - 2) * (m_Height - 2);
            int doneCells = 0;

            for(int x = 1; x < m_Width - 1; x++)
            {
                for(int y = 1; y < m_Height - 1; y++)
                {
                    var sim = new PrSimulator(new SimulationOptions(m_Width, m_Height, new IntPoint(x, y)));
                    while(CanContinue(sim.SimulationInfo))
                    {
                        if (CancelRequested())
                        {
                            return null;
                        }
                        sim.SimulateSteps(10);
                    }

                    double sum = 0;
                    for(int x_ = 0; x_ < m_Width; x_++)
                    {
                        sum += sim[x_, 0] * m_EdgeData[x_, 0];
                        sum += sim[x_, m_Height - 1] * m_EdgeData[x_, m_Height - 1];
                    }
                    for (int y_ = 1; y_ < m_Height - 1; y_++)
                    {
                        sum += sim[0, y_] * m_EdgeData[0, y_];
                        sum += sim[m_Width - 1, y_] * m_EdgeData[m_Width - 1, y_];
                    }

                    res[x, y] = sum;

                    doneCells++;
                    ReportProgress((int)((double)doneCells / totalCells * 100));
                }
            }

            return new GridData(res);
        }
    }
}
