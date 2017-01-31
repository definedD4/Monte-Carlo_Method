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
