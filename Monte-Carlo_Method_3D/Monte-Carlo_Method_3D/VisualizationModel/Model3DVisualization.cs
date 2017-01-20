using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.VisualizationModel
{
    public class Model3DVisualization : I3DModelVisualization
    {
        public GeometryModel3D Model { get; }

        public Model3DVisualization(GeometryModel3D model)
        {
            Model = model;
        }
    }
}