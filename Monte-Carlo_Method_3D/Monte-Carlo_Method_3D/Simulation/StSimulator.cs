using System;
using System.Security.Policy;
using Monte_Carlo_Method_3D.Util;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StSimulator
    {
        private static readonly List<Tuple<IntPoint, double>> MovePropabilities = new List<Tuple<IntPoint, double>>{
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

        public StSimulator(int width, int height, IntPoint startLocation)
        {
            Width = width;
            Height = height;
            StartLocation = startLocation;

            m_Data = new EdgeData(Width, Height);

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath, TotalSimTime);
        }

        public int Width { get; }
        public int Height { get; }
        public IntPoint StartLocation { get; }
        public long TotalSimulations { get; private set; }
        public double AverageTravelPath { get; private set; }
        public double TotalSimTime { get; private set; }

        public StSimulationInfo SimulationInfo { get; private set; }

        public void SimulateSteps(long steps)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<IntPoint> path = new List<IntPoint>();
            for (long i = 0; i < steps; i++)
            {
                IntPoint pos = StartLocation;
                int travelPath = 0;

                if (i == steps - 1) // last step
                {
                    path.Add(pos);
                }

                while (pos.InBoundsStrict(0, Width - 1, 0, Height - 1))
                {
                    pos += SelectRandomDirection();

                    travelPath++;
                    if (i == steps - 1) // last step
                    {
                        path.Add(pos);
                    }
                }
                m_Data[pos.X, pos.Y] += 1;

                AverageTravelPath = (AverageTravelPath * TotalSimulations + travelPath) / ++TotalSimulations;
            }
            stopwatch.Stop();
            TotalSimTime += stopwatch.Elapsed.TotalMilliseconds;

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath, TotalSimTime);
            LastPath = new StParticlePath(path);
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

        public StParticlePath LastPath { get; private set; }

        public void Reset()
        {
            m_Data = new EdgeData(Width, Height);
            TotalSimulations = 0;
            AverageTravelPath = 0;

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath, TotalSimTime);
            LastPath = null;
        }
    }
}