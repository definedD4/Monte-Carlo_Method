using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.DataModel;
using static System.Math;

namespace Monte_Carlo_Method_3D.VisualizationModel
{
    public class GridTableVisualization : IImageVisualization, IGridVisualization
    {
        public ImageSource Image { get; }
        public GridData Data { get; }

        public GridTableVisualization(ImageSource image, GridData data)
        {
            Image = image;
            Data = data;
        }

        public double? GetValueAtRelativeCoords(Point coords)
        {
            var idx = new GridIndex(
                (int)Floor(coords.Y * Data.Size.Height),
                (int)Floor(coords.X * Data.Size.Width));

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