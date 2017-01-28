using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Visualization
{
    public static class DrawingUtil
    {
        public static unsafe void DrawColorCell(byte* pixels, int x, int y, Color color, int bytesPerPixel, int stride)
        {
            int index = y * stride + x * bytesPerPixel;
            pixels[index] = color.B;
            pixels[index + 1] = color.G;
            pixels[index + 2] = color.R;
        }

        public static void DrawTableCell(DrawingContext drawingContext, int x, int y, string str, Brush foregroundBrush)
        {
            drawingContext.DrawText(new FormattedText(str,
                            CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 0.15, foregroundBrush),
                            new Point(x + 0.2f, y + 0.2f));
        }

        public static ImageSource DrawTable(GridData data, Color background, Color foreground, Pen gridPen)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(background), null, new Rect(new Size(data.Size.Columns, data.Size.Rows)));

                DrawGrid(drawingContext, gridPen, data.Size.Columns, data.Size.Rows);

                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    var str = Math.Round(data[idx], 5).ToString("G3");
                    DrawTableCell(drawingContext, idx.J, idx.I, str, foreground);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }

        public static void DrawGrid(DrawingContext drawingContext, Pen gridPen, int columns, int rows)
        {
            for (int i = 0; i <= rows; i++)
            {
                drawingContext.DrawLine(gridPen, new Point(0, i), new Point(columns, i));
            }

            for (int j = 0; j <= columns; j++)
            {
                drawingContext.DrawLine(gridPen, new Point(j, 0), new Point(j, rows));
            }
        }
    }
}
