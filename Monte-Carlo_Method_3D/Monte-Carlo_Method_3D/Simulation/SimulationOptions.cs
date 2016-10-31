using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class SimulationOptions
    {
        public SimulationOptions(int width, int height, IntPoint startLocation)
        {
            Width = width;
            Height = height;
            StartLocation = startLocation;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public IntPoint StartLocation { get; private set; }

        public static SimulationOptions FromSimulator(PrSimulator simulator)
        {
            return new SimulationOptions(simulator.Width, simulator.Height, simulator.StartLocation);
        }

        public static SimulationOptions FromSimulator(StSimulator simulator)
        {
            return new SimulationOptions(simulator.Width, simulator.Height, simulator.StartLocation);
        }
    }
}
