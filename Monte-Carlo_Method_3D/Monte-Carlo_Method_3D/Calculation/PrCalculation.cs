using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monte_Carlo_Method_3D.Simulation;
using JetBrains.Annotations;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util.AssertHelper;

namespace Monte_Carlo_Method_3D.Calculation
{
    public class PrCalculation : Calculation
    {
        private readonly EdgeData m_EdgeData;

        public PrCalculation([NotNull] CalculationConstraint constraint, [NotNull] EdgeData edgeData, IEnumerable<GridIndex> calculationMask)
            : base(constraint, calculationMask)
        {
            constraint.AssertNotNull(nameof(constraint));
            edgeData.AssertNotNull(nameof(edgeData));

            m_EdgeData = edgeData;
        }

        protected override GridData Simulate(IEnumerable<GridIndex> mask)
        {
            var maskList = mask.ToList();
            if (maskList.Count == 0)
            {
                maskList.AddRange(m_EdgeData.Inaccessable.EnumerateRegion());
            }

            GridData res = GridData.AllocateNew(m_EdgeData.Size);

            int totalCells = maskList.Count;
            int doneCells = 0;

            Parallel.ForEach(maskList, index =>
            {
                res[index] = CalcCell(index);

                doneCells++;
                ReportProgress((int) ((double) doneCells / totalCells * 100));
            });

            return res;
        }

        private double CalcCell(GridIndex coords)
        {
            var sim = new PrSimulator(new SimulationOptions(m_EdgeData.Size, coords));
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
            foreach (var idx in m_EdgeData.Bounds.EnumerateEdge())
            {
                sum += data[idx] * m_EdgeData[idx];
            }
            return sum;
        }
    }
}
