using System;
using System.Security.Policy;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StatMethodSimulator
    {
        private Random m_Random = new Random();

        private EdgeData m_Data;

        public int Width { get; }
        public int Height { get; }
        public IntPoint StartLocation { get; }
        public int TotalSimulations { get; private set; }
        public double AverageTravelPath { get; private set; }

        public StatMethodSimulator(int width, int height, IntPoint startLocation)
        {
            Width = width;
            Height = height;
            StartLocation = startLocation;

            m_Data = new EdgeData(Width, Height);
        }

        public void SimulateStep()
        {
            IntPoint pos = StartLocation;
            int travelPath = 0;
            while (pos.InBoundsStrict(0, Width - 1, 0, Height - 1))
            {
                int direction = m_Random.Next(0, 4); // 0 - right, 1 - up, 2 - left, 3 - down
                switch (direction)
                {
                    case 0:
                        pos += new IntPoint(1, 0);
                    break;
                    case 1:
                        pos += new IntPoint(0, 1);
                    break;
                    case 2:
                        pos += new IntPoint(-1, 0);
                    break;
                    case 3:
                        pos += new IntPoint(0, -1);
                    break;
                }
                travelPath++;
            }
            m_Data[pos.X, pos.Y] += 1;

            AverageTravelPath = (AverageTravelPath*TotalSimulations + travelPath)/++TotalSimulations;
        }

        public double this[int x, int y] => m_Data[x, y]/TotalSimulations;
    }
}