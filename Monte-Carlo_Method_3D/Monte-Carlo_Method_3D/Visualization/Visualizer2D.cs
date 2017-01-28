using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.VisualizationModel;
using System.Windows;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class Visualizer2D
    {
        private const int Dpi = 96;

        public Pallete Pallete { get; set; }

        public Brush BackgroundBrush { get; set; }

        public bool DrawGrid { get; set; }

        public Pen GridPen { get; set; }

        public Brush ForgroundBrush { get; set; }

        public Visualizer2D(Pallete pallete)
        {
            Pallete = pallete;
        }

        public GridTableVisualization GenerateTableVisualization(GridData data)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(BackgroundBrush, null, new Rect(0, 0, data.Size.Columns, data.Size.Rows));

                // Draw grid
                if (DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, GridPen, data.Size.Columns, data.Size.Rows);
                }

                // Draw values
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    DrawingUtil.DrawTableCell(drawingContext, idx.J, idx.I, FormattingUtil.FormatShort(data[idx]), ForgroundBrush);
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new GridTableVisualization(image, data);
        }

        public GridTableVisualization GenerateColorVisualization(GridData data)
        {

        }
    }
}
