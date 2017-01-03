using Monte_Carlo_Method_3D.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class PrSimulator
    {
        private const double DiagonalMovePropability = 1 / 20d;
        private const double HorizontalVerticalMovePropability = 1 / 5d;
        private double[,] m_Data;

        public int Width { get; }
        public int Height { get; }
        public IntPoint StartLocation { get; }
        public long Step { get; private set; }
        public double TotalSimTime { get; private set; }

        public PrSimulationInfo SimulationInfo { get; private set; }

        public PrSimulator(SimulationOptions options)
        {
            Width = options.Width;
            Height = options.Height;
            StartLocation = options.StartLocation;
            m_Data = new double[Width, Height];
            m_Data[StartLocation.X, StartLocation.Y] = 1;
            Step = 0;
            TotalSimTime = 0;
            SimulationInfo = new PrSimulationInfo(Step, TotalSimTime, 1, 0, 1);
        }

        public bool CanIndex(int x, int y) => new IntPoint(x, y).InBounds(0, Width - 1, 0, Height - 1);

        public double this[int x, int y] => m_Data[x, y];

        public void SimulateSteps(long steps = 1)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (long i = 0; i < steps; i++)
            {
                double[,] newData = new double[Width, Height];
                for (int x = 1; x < Width - 1; x++)
                {
                    for (int y = 1; y < Height - 1; y++)
                    {
                        newData[x + 1, y] += m_Data[x, y] * HorizontalVerticalMovePropability;
                        newData[x + 1, y + 1] += m_Data[x, y] * DiagonalMovePropability;
                        newData[x, y + 1] += m_Data[x, y] * HorizontalVerticalMovePropability;
                        newData[x - 1, y + 1] += m_Data[x, y] * DiagonalMovePropability;
                        newData[x - 1, y] += m_Data[x, y] * HorizontalVerticalMovePropability;
                        newData[x - 1, y - 1] += m_Data[x, y] * DiagonalMovePropability;
                        newData[x, y - 1] += m_Data[x, y] * HorizontalVerticalMovePropability;
                        newData[x + 1, y - 1] += m_Data[x, y] * DiagonalMovePropability;
                    }
                }

                for (int x = 0; x < Width; x++)
                {
                    newData[x, 0] += m_Data[x, 0];
                    newData[x, Height - 1] += m_Data[x, Height - 1];
                }

                for (int y = 1; y < Height - 1; y++)
                {
                    newData[0, y] += m_Data[0, y];
                    newData[Width - 1, y] += m_Data[Width - 1, y];
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
            m_Data = new double[Width, Height];
            Step = 0;
            TotalSimTime = 0;
            m_Data[StartLocation.X, StartLocation.Y] = 1;

            SimulationInfo = new PrSimulationInfo(Step, TotalSimTime, GetCenterSum(), GetEdgeSum(), GetTotalSum());
        }

        private double GetCenterSum()
        {
            double centerSum = 0;
            for(int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    centerSum += m_Data[x, y];
                }
            }
            return centerSum;
        }

        private double GetEdgeSum()
        {
            double edgeSum = 0;
            for (int x = 1; x < Width - 1; x++)
            {
                edgeSum += m_Data[x, 0];
                edgeSum += m_Data[x, Height - 1];
            }
            for (int y = 0; y < Height; y++)
            {
                edgeSum += m_Data[0, y];
                edgeSum += m_Data[Width - 1, y];
            }
            return edgeSum;
        }

        private double GetTotalSum()
        {
            double sum = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    sum += m_Data[x, y];
                }
            }
            return sum;
        }

        public double[,] GetData() => m_Data;
    }
}
