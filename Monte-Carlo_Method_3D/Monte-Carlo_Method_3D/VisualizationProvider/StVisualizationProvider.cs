using JetBrains.Annotations;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.VisualizationProvider
{
    public abstract class StVisualizationProvider
    {
        public abstract IVisualization ProvideVisualization([NotNull] StSimulator simulator);

        public static StVisualizationProvider Table() => new TableStVisualizationProvider();

        public static StVisualizationProvider Color() => new ColorStVisualizationProvider();

        private class TableStVisualizationProvider : StVisualizationProvider
        {
            private readonly Visualizer2D m_Visualizer = new Visualizer2D();

            public override IVisualization ProvideVisualization(StSimulator simulator)
            {
                return m_Visualizer.GenerateEdgeTableVisualization(simulator.GetData());
            }
        }

        public class ColorStVisualizationProvider : StVisualizationProvider
        {
            private readonly Visualizer2D m_Visualizer = new Visualizer2D();

            public override IVisualization ProvideVisualization(StSimulator simulator)
            {
                return m_Visualizer.GenerateEdgeColorVisualizationWithPath(simulator.GetData(), simulator.LastPath);
            }
        }
    }
}