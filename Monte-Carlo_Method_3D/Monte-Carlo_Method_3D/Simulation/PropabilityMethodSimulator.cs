using Monte_Carlo_Method_3D.Util;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class PropabilityMethodSimulator : INotifyPropertyChanged
    {
        private double[,] data;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Step { get; private set; }
        public IntPoint StartLocation { get; private set; }
        public int TotalSimTime { get; private set; }

        public PropabilityMethodSimulator(int width, int height, IntPoint startLocation)
        {
            Width = width;
            Height = height;
            StartLocation = startLocation;
            data = new double[width, height];
            data[startLocation.X, startLocation.Y] = 1;
            Step = 0;
            TotalSimTime = 0;
        }

        public double this[int x, int y]
        {
            get { return data[x, y]; }
        }

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
            for (int i = 1; i < Width - 1; i++)
            {
                newData[i, 0] = data[i, 0] + data[i, 1] / 5;
                if (i > 1)
                    newData[i, 0] += data[i - 1, 1] / 20;
                if (i < Width - 2)
                    newData[i, 0] += data[i + 1, 1] / 20;

                newData[i, Height - 1] = data[i, Height - 1] + data[i, Height - 2] / 5;
                if (i > 1)
                    newData[i, Height - 1] += data[i - 1, Height - 2] / 20;
                if (i < Width - 2)
                    newData[i, Height - 1] += data[i + 1, Height - 2] / 20;
            }
            for (int i = 1; i < Height - 1; i++)
            {
                newData[0, i] = data[0, i] + data[1, i] / 5;
                if (i > 1)
                    newData[0, i] += data[1, i - 1] / 20;
                if (i < Width - 2)
                    newData[0, i] += data[1, i + 1] / 20;

                newData[Width - 1, i] = data[Width - 1, i] + data[Width - 2, i] / 5;
                if (i > 1)
                    newData[Width - 1, i] += data[Width - 2, i - 1] / 20;
                if (i < Width - 2)
                    newData[Width - 1, i] += data[Width - 2, i + 1] / 20;
            }
            data = newData;
            Step++;
            stopwatch.Stop();
            TotalSimTime += stopwatch.Elapsed.Milliseconds;
            OnPropertyChanged("Step");
            OnPropertyChanged("TotalSimTime");

            CenterSum = GetCenterSum();
            EdgeSum = GetEdgeSum();
        }

        public void Reset()
        {
            data = new double[Width, Height];
            CenterSum = 1;
            EdgeSum = 0;
            TotalSimTime = 0;
            data[StartLocation.X, StartLocation.Y] = 1;
        }

        private double p_CenterSum = -1;
        public double CenterSum
        {
            get
            {
                return p_CenterSum;
            }
            private set
            {
                p_CenterSum = value;
                OnPropertyChanged(nameof(CenterSum));
            }
        }


        private double p_EdgeSum = -1;
        public double EdgeSum
        {
            get
            {
                return p_EdgeSum;
            }
            private set
            {
                p_EdgeSum = value;
                OnPropertyChanged(nameof(EdgeSum));
            }
        }

        private double GetCenterSum()
        {
            double centerSum = 0;
            for(int x = 1; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
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
            for (int y = 1; y < Width - 1; y++)
            {
                edgeSum += data[0, y];
                edgeSum += data[Width - 1, y];
            }
            return edgeSum;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name.");
            }

            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
