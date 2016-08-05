using System;
using System.Security.Policy;
using Monte_Carlo_Method_3D.Util;
using System.Collections.Generic;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StSimulator
    {
        private static List<Tuple<IntPoint, double>> MovePropabilities = new List<Tuple<IntPoint, double>>{
            Tuple.Create(new IntPoint(1, 0), 1d / 5d),
            Tuple.Create(new IntPoint(1, 1), 1d / 20d),
            Tuple.Create(new IntPoint(0, 1), 1d / 5d),
            Tuple.Create(new IntPoint(-1, 1), 1d / 20d),
            Tuple.Create(new IntPoint(-1, 0), 1d / 5d),
            Tuple.Create(new IntPoint(-1, -1), 1d / 20d),
            Tuple.Create(new IntPoint(0, -1), 1d / 5d),
            Tuple.Create(new IntPoint(1, -1), 1d / 20d)
        };

        private Random m_Random = new Random();

        private EdgeData m_Data;

        public int Width { get; }
        public int Height { get; }
        public IntPoint StartLocation { get; }
        public long TotalSimulations { get; private set; }
        public double AverageTravelPath { get; private set; }

        public StSimulationInfo SimulationInfo { get; private set; }

        public StSimulator(int width, int height, IntPoint startLocation)
        {
            Width = width;
            Height = height;
            StartLocation = startLocation;

            m_Data = new EdgeData(Width, Height);
        }

        public void SimulateSteps(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                IntPoint pos = StartLocation;
                int travelPath = 0;
                while (pos.InBoundsStrict(0, Width - 1, 0, Height - 1))
                {
                    pos += SelectRandomDirection();
                    travelPath++;
                }
                m_Data[pos.X, pos.Y] += 1;

                AverageTravelPath = (AverageTravelPath * TotalSimulations + travelPath) / ++TotalSimulations;
            }

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath);
        }

        private IntPoint SelectRandomDirection()
        {
            double diceRoll = m_Random.NextDouble();

            double cumulative = 0.0;
            for (int i = 0; i < MovePropabilities.Count; i++)
            {
                cumulative += MovePropabilities[i].Item2;
                if (diceRoll < cumulative)
                {
                    return MovePropabilities[i].Item1;
                }
            }
            return MovePropabilities[MovePropabilities.Count].Item1;
        }

        public double this[int x, int y] => m_Data[x, y]/TotalSimulations;
    }
}