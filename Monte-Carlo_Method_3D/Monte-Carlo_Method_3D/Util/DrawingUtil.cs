using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Util
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

        public static void DrawTableCell(DrawingContext drawingContext, int x, int y, string str, Color foregroundColor)
        {
            drawingContext.DrawText(new FormattedText(str,
                            CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 0.15, new SolidColorBrush(foregroundColor)),
                            new Point(x + 0.2f, y + 0.2f));
        }

        public static ImageSource DrawTable(GridData data, Color background, Color foreground)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(background), null, new Rect(new Size(data.Width, data.Height)));
                for (int i = 0; i < data.Width; i++)
                {
                    for (int j = 0; j < data.Height; j++)
                    {
                        var str = Math.Round(data[i, j], 5).ToString("G3");
                        DrawTableCell(drawingContext, i, j, str, foreground);
                    }
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
