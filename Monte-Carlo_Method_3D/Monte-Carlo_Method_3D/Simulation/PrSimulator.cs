using Monte_Carlo_Method_3D.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class PrSimulator
    {
        private double[,] data;

        public int Width { get; }
        public int Height { get; }
        public IntPoint StartLocation { get; }
        public int Step { get; private set; }
        public int TotalSimTime { get; private set; }
        public SimulationInfo SimulationInfo { get; private set; }

        public PrSimulator(int width, int height, IntPoint startLocation)
        {
            Width = width;
            Height = height;
            StartLocation = startLocation;
            data = new double[width, height];
            data[startLocation.X, startLocation.Y] = 1;
            Step = 0;
            TotalSimTime = 0;
            SimulationInfo = new SimulationInfo(Step, TotalSimTime, 1, 0, 1);
        }

        public double this[int x, int y] => data[x, y];

        public void SimulateStep()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double[,] newData = new double[Width, Height];
            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    double s = 0;
                    if (x > 1)
                        s += data[x - 1, y] / 5;
                    if (x < Width - 2)
                        s += data[x + 1, y] / 5;
                    if (y > 1)
                        s += data[x, y - 1] / 5;
                    if (y < Height - 2)
                        s += data[x, y + 1] / 5;
                    if (x > 1 && y > 1)
                        s += data[x - 1, y - 1] / 20;
                    if (x < Width - 2 && y > 1)
                        s += data[x + 1, y - 1] / 20;
                    if (x > 1 && y < Height - 2)
                        s += data[x - 1, y + 1] / 20;
                    if (x < Width - 2 && y < Height - 2)
                        s += data[x + 1, y + 1] / 20;
                    newData[x, y] = s;
                }
            }
            for (int x = 1; x < Width - 1; x++)
            {
                newData[x, 0] = data[x, 0] + data[x, 1] / 5;
                if (x > 1)
                    newData[x, 0] += data[x - 1, 1] / 20;
                if (x < Width - 2)
                    newData[x, 0] += data[x + 1, 1] / 20;

                newData[x, Height - 1] = data[x, Height - 1] + data[x, Height - 2] / 5;
                if (x > 1)
                    newData[x, Height - 1] += data[x - 1, Height - 2] / 20;
                if (x < Width - 2)
                    newData[x, Height - 1] += data[x + 1, Height - 2] / 20;
            }
            for (int y = 1; y < Height - 1; y++)
            {
                newData[0, y] = data[0, y] + data[1, y] / 5; // right
                if (y > 1)
                    newData[0, y] += data[1, y - 1] / 20; // upper right
                if (y < Width - 2)
                    newData[0, y] += data[1, y + 1] / 20; // lower right

                newData[Width - 1, y] = data[Width - 1, y] + data[Width - 2, y] / 5; // left
                if (y > 1)
                    newData[Width - 1, y] += data[Width - 2, y - 1] / 20; // upper left
                if (y < Width - 2)
                    newData[Width - 1, y] += data[Width - 2, y + 1] / 20; // lower left
            }
            newData[0, 0] = data[0, 0] + data[1, 1] / 20;
            newData[0, Height - 1] = data[0, 0] + data[1, 1] / 20;
            newData[Width - 1, 0] = data[0, 0] + data[1, 1] / 20;
            newData[Width - 1, Height - 1] = data[0, 0] + data[1, 1] / 20;

            data = newData;
            Step++;
            stopwatch.Stop();
            TotalSimTime += stopwatch.Elapsed.Milliseconds;

            SimulationInfo = new SimulationInfo(Step, TotalSimTime, GetCenterSum(), GetEdgeSum(), GetTotalSum());
        }

        public void Reset()
        {
            data = new double[Width, Height];
            TotalSimTime = 0;
            data[StartLocation.X, StartLocation.Y] = 1;
        }

        private double GetCenterSum()
        {
            double centerSum = 0;
            for(int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    centerSum += data[x, y];
                }
            }
            return centerSum;
        }

        private double GetEdgeSum()
        {
            double edgeSum = 0;
            for (int x = 1; x < Width - 1; x++)
            {
                edgeSum += data[x, 0];
                edgeSum += data[x, Height - 1];
            }
            for (int y = 0; y < Width; y++)
            {
                edgeSum += data[0, y];
                edgeSum += data[Width - 1, y];
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
                    sum += data[x, y];
                }
            }
            return sum;
        }

        public double[,] GetData() => data;
    }
}
