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
using System.Diagnostics;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class StVisualizer
    {
        private const int Dpi = 96;

        private Pallete m_Pallete;
        private StSimulator m_Simulator;

        public StVisualizer(StSimulator simulator, Pallete pallete)
        {
            m_Simulator = simulator;
            m_Pallete = pallete;          
        }

        public int Width => m_Simulator.Width;
        public int Height => m_Simulator.Height;

        public Color BackgroundColor { get; set; } = Colors.White;
        public Color ForegroundColor { get; set; } = Colors.Black;
        public Color PathColor { get; set; } = Colors.Black;
        public Color StartPointColor { get; set; } = Colors.Black;

        public bool DrawPath { get; set; } = true;
        public bool DrawStartPoint { get; set; } = true;

        public ImageSource GenerateColorTexture()
        {
            WriteableBitmap texture = new WriteableBitmap(Width, Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = texture.Format.BitsPerPixel / 8;
            int stride = texture.BackBufferStride;

            texture.Lock();
            unsafe
            {
                byte* bytes = (byte*)texture.BackBuffer.ToPointer();

                for(int x = 0; x < Width; x++)
                {
                    for(int y = 0; y < Height; y++)
                    {
                        if(m_Simulator.CanIndex(x, y))
                        {
                            DrawingUtil.DrawColorCell(bytes, x, y, m_Pallete.GetColor(m_Simulator[x, y]), bytesPerPixel, stride);
                        }
                        else
                        {
                            DrawingUtil.DrawColorCell(bytes, x, y, BackgroundColor, bytesPerPixel, stride);
                        }
                    }
                }
                texture.AddDirtyRect(new Int32Rect(0, 0, texture.PixelWidth, texture.PixelHeight));
                texture.Unlock();
            }
            texture.Freeze();

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(texture, new Rect(new Size(Width, Height)));

                if (DrawPath && m_Simulator.LastPath != null)
                {
                    var pen = new Pen(new SolidColorBrush(PathColor), 0.1D);
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
                if(DrawStartPoint)
                    drawingContext.DrawEllipse(new SolidColorBrush(StartPointColor), null, new Point(startLoc.X + 0.5, startLoc.Y + 0.5), 0.3, 0.3);
            }
            return new DrawingImage(visual.Drawing);
        }

        public ImageSource GenerateTableTexture()
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(BackgroundColor), null, new Rect(new Size(Width, Height)));
                for (int x = 0; x < Width; x++)
                {
                    DrawingUtil.DrawTableCell(drawingContext, x,          0, Math.Round(m_Simulator[x,          0], 5).ToString("E2"), ForegroundColor);
                    DrawingUtil.DrawTableCell(drawingContext, x, Height - 1, Math.Round(m_Simulator[x, Height - 1], 5).ToString("E2"), ForegroundColor);
                }
                for (int y = 0; y < Height; y++)
                {
                    DrawingUtil.DrawTableCell(drawingContext,         0, y, Math.Round(m_Simulator[        0, y], 5).ToString("E2"), ForegroundColor);
                    DrawingUtil.DrawTableCell(drawingContext, Width - 1, y, Math.Round(m_Simulator[Width - 1, y], 5).ToString("E2"), ForegroundColor);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
