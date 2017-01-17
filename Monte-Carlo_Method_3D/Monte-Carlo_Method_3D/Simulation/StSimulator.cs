using System;
using System.Security.Policy;
using Monte_Carlo_Method_3D.Util;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StSimulator
    {
        private static readonly List<Tuple<GridIndex, double>> MovePropabilities = new List<Tuple<GridIndex, double>>{
            Tuple.Create(new GridIndex(1, 0), 1d / 5d),
            Tuple.Create(new GridIndex(1, 1), 1d / 20d),
            Tuple.Create(new GridIndex(0, 1), 1d / 5d),
            Tuple.Create(new GridIndex(-1, 1), 1d / 20d),
            Tuple.Create(new GridIndex(-1, 0), 1d / 5d),
            Tuple.Create(new GridIndex(-1, -1), 1d / 20d),
            Tuple.Create(new GridIndex(0, -1), 1d / 5d),
            Tuple.Create(new GridIndex(1, -1), 1d / 20d)
        };

        private readonly Random m_Random = new Random();

        private EdgeData m_Data;
        private EdgeData m_ProccesedData;

        public StSimulator(SimulationOptions options)
        {
            Size = options.Size;
            StartLocation = options.StartLocation;

            m_Data = EdgeData.AllocateNew(Size);
            m_ProccesedData = EdgeData.AllocateLike(m_Data);

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath, TotalSimTime);
        }

        public GridSize Size { get; }
        public GridIndex StartLocation { get; }
        public long TotalSimulations { get; private set; }
        public double AverageTravelPath { get; private set; }
        public double TotalSimTime { get; private set; }

        public StSimulationInfo SimulationInfo { get; private set; }

        public void SimulateSteps(long steps = 1)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<GridIndex> path = new List<GridIndex>();
            for (long step = 0; step < steps; step++)
            {
                GridIndex pos = StartLocation;
                int travelPath = 0;

                if (step == steps - 1) // last step
                {
                    path.Add(pos); // record path
                }

                while (m_Data.Inaccessable.IsInside(pos))
                {
                    pos += SelectRandomDirection();

                    travelPath++;
                    if (step == steps - 1) // last step
                    {
                        path.Add(pos);
                    }
                }
                m_Data[pos] += 1;

                AverageTravelPath = (AverageTravelPath * TotalSimulations + travelPath) / ++TotalSimulations;
            }
            stopwatch.Stop();
            ProcessData();

            TotalSimTime += stopwatch.Elapsed.TotalMilliseconds;

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath, TotalSimTime);
            LastPath = new StParticlePath(path);
        }

        private void ProcessData()
        {
            m_ProccesedData = EdgeData.AllocateLike(m_Data);
            foreach (var i in m_ProccesedData.Bounds.EnumerateEdge())
            {
                m_ProccesedData[i] = m_Data[i] / TotalSimulations;
            }
        }

        private GridIndex SelectRandomDirection()
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

        public StParticlePath LastPath { get; private set; }

        public void Reset()
        {
            m_Data = EdgeData.AllocateLike(m_Data);
            ProcessData();
            TotalSimulations = 0;
            AverageTravelPath = 0;
            TotalSimTime = 0;

            SimulationInfo = new StSimulationInfo(TotalSimulations, AverageTravelPath, TotalSimTime);
            LastPath = null;
        }

        public EdgeData GetData() => m_ProccesedData;
    }
}