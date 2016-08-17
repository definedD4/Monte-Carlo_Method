using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

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

        public static void DrawTableCell(DrawingContext drawingContext, int x, int y, string str)
        {
            drawingContext.DrawText(new FormattedText(str,
                            CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 0.15, Brushes.White),
                            new Point(x + 0.3f, y + 0.3f));
        }
    }
}
