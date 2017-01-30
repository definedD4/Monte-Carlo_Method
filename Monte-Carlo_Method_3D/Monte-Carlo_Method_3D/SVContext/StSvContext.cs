using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.SVContext
{
    public abstract class StSvContext
    {
        public SimulationOptions Options { get; }

        public Pallete Pallete { get; }

        public StSimulator Simulator { get; }

        private StSvContext(SimulationOptions options, Pallete pallete)
        {
            Options = options;
            Pallete = pallete;
            Simulator = new StSimulator(options);
        }

        public abstract IVisualization ProvideVisualization();

        public abstract StSvContext Clone(SimulationOptions options);

        // Implementations

        // Table

        public static StSvContext Table(SimulationOptions options, Pallete pallete) =>
            new StSvContext.TableStSvContext(options, pallete);

        protected class TableStSvContext : StSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public TableStSvContext(SimulationOptions options, Pallete pallete) : base(options, pallete)
            {
                m_Visualizer = new Visualizer2D() {Pallete = pallete};
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateEdgeTableVisualization(Simulator.GetData());
            }

            public override StSvContext Clone(SimulationOptions options)
            {
                return new TableStSvContext(options, Pallete);
            }
        }

        // Color

        public static StSvContext Color(SimulationOptions options, Pallete pallete) =>
            new StSvContext.ColorStSvContext(options, pallete);

        protected class ColorStSvContext : StSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public ColorStSvContext(SimulationOptions options, Pallete pallete) : base(options, pallete)
            {
                m_Visualizer = new Visualizer2D() {Pallete = pallete};
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateEdgeColorVisualizationWithPath(Simulator.GetData(), Simulator.LastPath);
            }

            public override StSvContext Clone(SimulationOptions options)
            {
                return new ColorStSvContext(options, Pallete);
            }
        }
    }
}