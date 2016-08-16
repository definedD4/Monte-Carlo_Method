using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class StVisualizer
    {
        private IPallete m_Pallete;
        private StSimulator m_Simulator;

        public StVisualizer(StSimulator simulator, IPallete pallete)
        {
            m_Simulator = simulator;
            m_Pallete = pallete;          
        }

        public int Width => m_Simulator.Width;
        public int Height => m_Simulator.Height;

        public ImageSource GenerateColorTexture(bool drawPath = false)
        {
            PixelFormat format = PixelFormats.Bgr24;
            int bytesPerPixel = format.BitsPerPixel / 8;
            int dpi = 96;
            int stride = Width * bytesPerPixel;
            byte[] bytes = new byte[Height * stride];
            for (int x = 0; x < Width; x++)
            {
                DrawCell(x, 0, stride, bytesPerPixel, bytes);
                DrawCell(x, Height - 1, stride, bytesPerPixel, bytes);
            }
            for (int y = 1; y < Height; y++)
            {
                DrawCell(0, y, stride, bytesPerPixel, bytes);
                DrawCell(Width - 1, y, stride, bytesPerPixel, bytes);
            }
            BitmapSource texture = BitmapSource.Create(Width, Height, dpi, dpi, format, null, bytes, stride);
            texture.Freeze();

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(texture, new Rect(new Size(Width, Height)));

                if (drawPath && m_Simulator.LastPath != null)
                {
                    var pen = new Pen(Brushes.Purple, 0.1D);
                    pen.Freeze();

                    var geometry = new StreamGeometry();
                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        var points = m_Simulator.LastPath.Points.Select(p => new Point(p.X + 0.5, p.Y + 0.5));
                        ctx.BeginFigure(points.First(), true, false);
                        ctx.PolyLineTo(points.Skip(1).ToList(), true, true);
                    }
                    geometry.Freeze();

                    drawingContext.DrawGeometry(null, pen, geometry);
                }

                IntPoint startLoc = m_Simulator.StartLocation;
                drawingContext.DrawEllipse(Brushes.Purple, null, new Point(startLoc.X + 0.5, startLoc.Y + 0.5), 0.3, 0.3);
            }
            return new DrawingImage(visual.Drawing);
        }

        private void DrawCell(int x, int y, int stride, int bytesPerPixel, byte[] bytes)
        {
                    Color color = m_Pallete.GetColor(m_Simulator[x, y]);

                    int index = y * stride + x * bytesPerPixel;
                    bytes[index] = color.B;
                    bytes[index + 1] = color.G;
                    bytes[index + 2] = color.R;
        }

        public ImageSource GenerateTableTexture()
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Black, null, new Rect(new Size(Width, Height)));
                for (int x = 0; x < Width; x++)
                {
                    DrawTableCell(x, 0, drawingContext);
                    DrawTableCell(x, Height - 1, drawingContext);
                }
                for (int y = 0; y < Height; y++)
                {
                    DrawTableCell(0, y, drawingContext);
                    DrawTableCell(Width - 1, y, drawingContext);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }

        private void DrawTableCell(int i, int j, DrawingContext drawingContext)
        {
            double val = m_Simulator[i, j];
            drawingContext.DrawText(new FormattedText(Math.Round(val, 5).ToString("E2"),
                            CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 0.15, Brushes.White),
                            new Point(i + 0.3f, j + 0.3f));
        }
    }
}
