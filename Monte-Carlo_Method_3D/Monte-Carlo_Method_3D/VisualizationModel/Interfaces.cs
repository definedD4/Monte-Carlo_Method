using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.VisualizationModel
{
    public interface IVisualization
    {
        
    }

    public interface IImageVisualization : IVisualization
    {
        ImageSource Image { get; }
    }

    public interface I3DModelVisualization : IVisualization
    {
        GeometryModel3D Model { get; }
    }

    public interface IGridVisualization : IVisualization
    {
        double? GetValueAtRelativeCoords(Point coords);
    }
}