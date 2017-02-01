using JetBrains.Annotations;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.Visualization.GraphMesh;
using Monte_Carlo_Method_3D.Visualization.GraphMesh.Factory;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.VisualizationProvider
{
    public abstract class GridDataVisualizationProvider
    {
        public abstract IVisualization ProvideVisualization([NotNull] GridData data);

        public static GridDataVisualizationProvider Table() => new TableVisualizationProvider();

        public static GridDataVisualizationProvider Color() => new ColorVisualizationProvider();

        public static GridDataVisualizationProvider Model3D(GridSize size) => new Model3DVisualizationProvider(size);

        private class TableVisualizationProvider : GridDataVisualizationProvider
        {
            private readonly Visualizer2D m_Visualizer = new Visualizer2D();

            public override IVisualization ProvideVisualization(GridData data)
            {
                return m_Visualizer.GenerateTableVisualization(data);
            }
        }

        private class ColorVisualizationProvider : GridDataVisualizationProvider
        {
            private readonly Visualizer2D m_Visualizer = new Visualizer2D();

            public override IVisualization ProvideVisualization(GridData data)
            {
                return m_Visualizer.GenerateColorVisualization(data);
            }
        }

        private class Model3DVisualizationProvider : GridDataVisualizationProvider
        {
            private readonly Visualizer3D m_Visualizer;

            public Model3DVisualizationProvider(GridSize size)
            {
                m_Visualizer = new Visualizer3D(size);
            }

            public override IVisualization ProvideVisualization(GridData data)
            {
                return m_Visualizer.GenerateModel3DVisualization(data);
            }
        }
    }
}