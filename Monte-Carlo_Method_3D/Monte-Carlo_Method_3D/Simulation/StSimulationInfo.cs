using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StSimulationInfo
    {
        public StSimulationInfo(long totalSimulations, double averageTravelPath)
        {
            TotalSimulations = totalSimulations;
            AverageTravelPath = averageTravelPath;
        }

        public long TotalSimulations { get; private set; }
        public string TotalSimulationsFormated
        {
            get
            {
                if (TotalSimulations < 10000) return TotalSimulations.ToString();
                if (TotalSimulations < 10000000) return Math.Round((double)TotalSimulations / 1000d, 2).ToString() + "k";
                else return Math.Round((double)TotalSimulations / 1000000d, 2).ToString() + "M";
            }
        }

        public double AverageTravelPath { get; private set; }
        public string AverageTravelPathFormated
        {
            get
            {
                return Math.Round(AverageTravelPath, 4).ToString();
            }
        }
    }
}
