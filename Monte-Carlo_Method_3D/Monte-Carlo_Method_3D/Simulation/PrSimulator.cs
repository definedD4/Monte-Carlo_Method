using Monte_Carlo_Method_3D.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class PrSimulator
    {
        private const double DiagonalMovePropability = 1 / 20d;
        private const double HorizontalVerticalMovePropability = 1 / 5d;
        private GridData m_Data;

        public GridSize Size { get; }
        public GridIndex StartLocation { get; }
        public long Step { get; private set; }
        public double TotalSimTime { get; private set; }

        public PrSimulationInfo SimulationInfo { get; private set; }

        public PrSimulator(SimulationOptions options)
        {
            Size = options.Size;
            StartLocation = options.StartLocation;
            m_Data = GridData.AllocateNew(Size);
            m_Data[StartLocation] = 1;
            Step = 0;
            TotalSimTime = 0;
            SimulationInfo = new PrSimulationInfo(Step, TotalSimTime, 1, 0, 1);
        }

        public void SimulateSteps(long steps = 1)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (long step = 0; step < steps; step++)
            {
                GridData newData = GridData.AllocateLike(m_Data);
                foreach (var idx in m_Data.Bounds.Shrink(1).EnumerateRegion())
                {
                    newData[idx.Right()      ] += m_Data[idx] * HorizontalVerticalMovePropability;
                    newData[idx.TopRight()   ] += m_Data[idx] * DiagonalMovePropability;
                    newData[idx.Top()        ] += m_Data[idx] * HorizontalVerticalMovePropability;
                    newData[idx.TopLeft()    ] += m_Data[idx] * DiagonalMovePropability;
                    newData[idx.Left()       ] += m_Data[idx] * HorizontalVerticalMovePropability;
                    newData[idx.BottomLeft() ] += m_Data[idx] * DiagonalMovePropability;
                    newData[idx.Bottom()     ] += m_Data[idx] * HorizontalVerticalMovePropability;
                    newData[idx.BottomRight()] += m_Data[idx] * DiagonalMovePropability;
                }

                foreach (var idx in m_Data.Bounds.EnumerateEdge())
                {
                    newData[idx] += m_Data[idx];
                }

                m_Data = newData;
                Step++;
            }
            stopwatch.Stop();
            TotalSimTime += stopwatch.Elapsed.TotalMilliseconds;

            SimulationInfo = new PrSimulationInfo(Step, TotalSimTime, GetCenterSum(), GetEdgeSum(), GetTotalSum());
        }

        public void Reset()
        {
            m_Data = GridData.AllocateLike(m_Data);
            Step = 0;
            TotalSimTime = 0;
            m_Data[StartLocation] = 1;

            SimulationInfo = new PrSimulationInfo(Step, TotalSimTime, GetCenterSum(), GetEdgeSum(), GetTotalSum());
        }

        private double GetCenterSum()
        {
            double centerSum = 0;
            foreach (var idx in m_Data.Bounds.Shrink(1).EnumerateRegion())
            {
                centerSum += m_Data[idx];
            }
            return centerSum;
        }

        private double GetEdgeSum()
        {
            double edgeSum = 0;
            foreach (var idx in m_Data.Bounds.EnumerateEdge())
            {
                edgeSum += m_Data[idx];
            }
            return edgeSum;
        }

        private double GetTotalSum()
        {
            double sum = 0;
            foreach (var idx in m_Data.Bounds.EnumerateRegion())
            {
                sum += m_Data[idx];
            }
            return sum;
        }

        public GridData GetData() => m_Data;
    }
}
