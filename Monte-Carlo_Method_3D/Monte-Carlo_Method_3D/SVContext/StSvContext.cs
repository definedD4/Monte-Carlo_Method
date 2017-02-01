using JetBrains.Annotations;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.SVContext
{
    public abstract class StSvContext
    {
        public SimulationOptions SimulationOptions { get; }

        public StSimulator Simulator { get; }

        private StSvContext(SimulationOptions simulationOptions)
        {
            SimulationOptions = simulationOptions;
            Simulator = new StSimulator(simulationOptions);
        }

        public abstract IVisualization ProvideVisualization();

        public abstract StSvContext Clone([NotNull] SimulationOptions options);

        // Implementations

        // Table

        public static StSvContext Table(SimulationOptions options) =>
            new StSvContext.TableStSvContext(options);

        private class TableStSvContext : StSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public TableStSvContext(SimulationOptions simulationOptions) : base(simulationOptions)
            {
                m_Visualizer = new Visualizer2D();
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateEdgeTableVisualization(Simulator.GetData());
            }

            public override StSvContext Clone(SimulationOptions options)
            {
                return new TableStSvContext(options);
            }
        }

        // Color

        public static StSvContext Color(SimulationOptions options) =>
            new StSvContext.ColorStSvContext(options);

        private class ColorStSvContext : StSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public ColorStSvContext(SimulationOptions simulationOptions) : base(simulationOptions)
            {
                m_Visualizer = new Visualizer2D();
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateEdgeColorVisualizationWithPath(Simulator.GetData(), Simulator.LastPath);
            }

            public override StSvContext Clone(SimulationOptions options)
            {
                return new ColorStSvContext(options);
            }
        }
    }
}