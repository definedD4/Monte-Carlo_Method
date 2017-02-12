using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class SimulationOptions
    {
        public SimulationOptions(GridSize size, GridIndex startLocation)
        {
            Size = size;
            StartLocation = startLocation;
        }

        public GridSize Size { get; }
        public GridIndex StartLocation { get; private set; }

        public static SimulationOptions FromSimulator(PrSimulator simulator)
        {
            return new SimulationOptions(simulator.Size, simulator.StartLocation);
        }

        public static SimulationOptions FromSimulator(StSimulator simulator)
        {
            return new SimulationOptions(simulator.Size, simulator.StartLocation);
        }
    }
}
