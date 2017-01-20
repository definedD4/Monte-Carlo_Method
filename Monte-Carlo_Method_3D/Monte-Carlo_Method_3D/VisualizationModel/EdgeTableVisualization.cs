using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.DataModel;
using static System.Math;

namespace Monte_Carlo_Method_3D.VisualizationModel
{
    public class EdgeTableVisualization : IImageVisualization, IGridVisualization
    {
        public ImageSource Image { get; }
        public EdgeData Data { get; }

        public EdgeTableVisualization(ImageSource image, EdgeData data)
        {
            Image = image;
            Data = data;
        }

        public double? GetValueAtRelativeCoords(Point coords)
        {
            var idx = new GridIndex(
                (int)Floor(coords.Y * Data.Size.Rows),
                (int)Floor(coords.X * Data.Size.Columns));

            if (Data.CanIndex(idx))
            {
                return Data[idx];
            }
            else
            {
                return null;
            }
        }
    }
}