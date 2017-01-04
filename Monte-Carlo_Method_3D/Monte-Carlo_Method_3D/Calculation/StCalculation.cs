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
    public class StCalculation : Calculation
    {
        private readonly int m_Width;
        private readonly int m_Height;
        private readonly GridData m_EdgeData;

        public StCalculation(ICalculationConstraint constraint, int width, int height, GridData edgeData, IEnumerable<GridIndex> calculationMask) : base(constraint, calculationMask)
        {
            m_Width = width;
            m_Height = height;
            m_EdgeData = edgeData;
        }

        protected override GridData Simulate(IEnumerable<GridIndex> _mask)
        {
            var mask = _mask.ToList();
            if (mask.Count == 0)
            {
                for (int x = 1; x < m_Width - 1; x++)
                {
                    for (int y = 1; y < m_Height - 1; y++)
                    {
                        mask.Add(new GridIndex(x, y));
                    }
                }
            }

            double[,] res = new double[m_Width, m_Height];

            int totalCells = mask.Count;
            int doneCells = 0;

            mask.ForEach(
                p =>
                {
                    res[p.I, p.J] = CalcCell(p);

                    doneCells++;
                    ReportProgress((int)((double)doneCells / totalCells * 100));
                });


            return GridData.FromArray(res);
        }

        private double CalcCell(GridIndex coords)
        {
            var sim = new StSimulator(new SimulationOptions(new GridSize(m_Height, m_Width), coords));
            while (CanContinue(sim.SimulationInfo))
            {
                if (CancelRequested())
                {
                    return double.NaN;
                }
                sim.SimulateSteps(10);
            }

            var data = sim.GetData();
            double sum = 0;
            for (int x_ = 0; x_ < m_Width; x_++)
            {
                sum += data[x_, 0] * m_EdgeData[x_, 0];
                sum += data[x_, m_Height - 1] * m_EdgeData[x_, m_Height - 1];
            }
            for (int y_ = 1; y_ < m_Height - 1; y_++)
            {
                sum += data[0, y_] * m_EdgeData[0, y_];
                sum += data[m_Width - 1, y_] * m_EdgeData[m_Width - 1, y_];
            }
            return sum;
        }
    }
}
