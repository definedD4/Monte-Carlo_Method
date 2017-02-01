using JetBrains.Annotations;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.Visualization.GraphMesh;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.SVContext
{
    /// <summary>
    /// Bundles together simulator and visualizer.
    /// </summary>
    public abstract class PrSvContext
    {
        public SimulationOptions SimulationOptions { get; }

        public PrSimulator Simulator { get; }

        private PrSvContext(SimulationOptions simulationOptions)
        {
            SimulationOptions = simulationOptions;
            Simulator = new PrSimulator(simulationOptions);
        }

        public abstract IVisualization ProvideVisualization();

        public abstract PrSvContext Clone([NotNull] SimulationOptions options);
        
        // Implementations

        // Table

        public static PrSvContext Table(SimulationOptions options) => 
            new TablePrSvContext(options);

        private class TablePrSvContext : PrSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public TablePrSvContext(SimulationOptions simulationOptions) : base(simulationOptions)
            {
                m_Visualizer = new Visualizer2D();
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateTableVisualization(Simulator.GetData());
            }

            public override PrSvContext Clone(SimulationOptions options)
            {
                return new TablePrSvContext(options);
            }
        }

        // Color

        public static PrSvContext Color(SimulationOptions options) =>
            new ColorPrSvContext(options);

        private class ColorPrSvContext : PrSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public ColorPrSvContext(SimulationOptions simulationOptions) : base(simulationOptions)
            {
                m_Visualizer = new Visualizer2D();
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateColorVisualization(Simulator.GetData());
            }

            public override PrSvContext Clone(SimulationOptions options)
            {
                return new ColorPrSvContext(options);
            }
        }

        // Model 3D

        public static PrSvContext Model3D(SimulationOptions options) =>
            new Model3DPrSvContext(options);

        private class Model3DPrSvContext : PrSvContext
        {
            private readonly Visualizer3D m_Visualizer;

            public Model3DPrSvContext(SimulationOptions simulationOptions) : base(simulationOptions)
            {
                m_Visualizer = new Visualizer3D(simulationOptions.Size);
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateModel3DVisualization(Simulator.GetData());
            }

            public override PrSvContext Clone(SimulationOptions options)
            {
                return new Model3DPrSvContext(options);
            }
        }
    }
}