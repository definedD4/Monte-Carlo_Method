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
        public int ImageWidth => Width * PixelsPerCell;
        public int ImageHeight => Height * PixelsPerCell;

        public int PixelsPerCell { get; set; } = 1;

        public ImageSource GenerateColorTexture(bool drawPath = false)
        {
            PixelFormat format = PixelFormats.Bgr24;
            int bytesPerPixel = format.BitsPerPixel / 8;
            int dpi = 96;
            int stride = ImageWidth * bytesPerPixel;
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
            BitmapSource texture = BitmapSource.Create(ImageWidth, ImageHeight, dpi, dpi, PixelFormats.Bgr24, null, bytes, stride);
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(texture, new Rect(new Size(ImageWidth, ImageHeight)));

                if (m_Simulator.LastPath != null)
                {
                    StParticlePath path = m_Simulator.LastPath;

                    IntPoint prev = path.Points[0];
                    for (int i = 1; i < path.Points.Count; i++)
                    {
                        IntPoint curr = path.Points[i];

                        drawingContext.DrawLine(new Pen(Brushes.Purple, 1d / (double)PixelsPerCell * 0.1),
                            new Point(prev.X * PixelsPerCell + 0.5, prev.Y * PixelsPerCell + 0.5), new Point(curr.X * PixelsPerCell + 0.5, curr.Y * PixelsPerCell + 0.5));

                        prev = curr;
                    }
                }

                IntPoint startLoc = m_Simulator.StartLocation;
                drawingContext.DrawEllipse(Brushes.Purple, null, new Point(startLoc.X + 0.5, startLoc.Y + 0.5), 0.3, 0.3);
            }
            return new DrawingImage(visual.Drawing);
        }

        private void DrawCell(int x, int y, int stride, int bytesPerPixel, byte[] bytes)
        {
            for (int _x = 0; _x < PixelsPerCell; _x++)
            {
                for (int _y = 0; _y < PixelsPerCell; _y++)
                {
                    Color color = m_Pallete.GetColor(m_Simulator[x, y]);

                    int index = (y * PixelsPerCell + _y) * stride + (x * PixelsPerCell + _x) * bytesPerPixel;
                    bytes[index] = color.B;
                    bytes[index + 1] = color.G;
                    bytes[index + 2] = color.R;
                }
            }
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
                            new Point(i * PixelsPerCell + PixelsPerCell * 0.3f, j * PixelsPerCell + PixelsPerCell * 0.3f));
        }
    }
}
