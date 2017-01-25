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
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class StVisualizer
    {
        private const int Dpi = 96;

        private Pallete m_Pallete;
        private GridIndex m_StartLocation;

        public StVisualizer(GridSize size, GridIndex startLocation, Pallete pallete)
        {
            Size = size;
            Width = Size.Columns;
            Height = Size.Rows;
            m_StartLocation = startLocation;
            m_Pallete = pallete;          
        }

        public GridSize Size { get; }
        public int Width { get; }
        public int Height { get; }

        public Color BackgroundColor { get; set; } = Colors.White;
        public Color ForegroundColor { get; set; } = Colors.Black;
        public Color PathColor { get; set; } = Colors.Black;
        public Color StartPointColor { get; set; } = Colors.Black;
        public Color GridColor { get; set; } = Colors.DarkGray;

        public bool DrawPath { get; set; } = true;
        public bool DrawStartPoint { get; set; } = true;

        public EdgeTableVisualization GenerateTableVisualization(EdgeData data)
        {
            return new EdgeTableVisualization(GenerateTableTexture(data), data);
        }

        public EdgeTableVisualization GenerateColorVisualization(EdgeData data, StParticlePath path)
        {
            return new EdgeTableVisualization(GenerateColorTexture(data, path), data);
        }

        public ImageSource GenerateColorTexture(EdgeData data, StParticlePath path)
        {
            // Generate texture
            WriteableBitmap texture = new WriteableBitmap(Width, Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = texture.Format.BitsPerPixel / 8;
            int stride = texture.BackBufferStride;

            texture.Lock();
            unsafe
            {
                byte* bytes = (byte*)texture.BackBuffer.ToPointer();

                foreach (var idx in data.Bounds.EnumerateEdge())
                {
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I, m_Pallete.GetColor(data[idx]), bytesPerPixel, stride);
                }

                foreach (var idx in data.Inaccessable.EnumerateRegion())
                {
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I, BackgroundColor, bytesPerPixel, stride);
                }

                texture.AddDirtyRect(new Int32Rect(0, 0, texture.PixelWidth, texture.PixelHeight));
                texture.Unlock();
            }
            texture.Freeze();
            
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(texture, new Rect(new Size(Width, Height)));

                var gridPen = new Pen(new SolidColorBrush(GridColor), 0.01D);
                DrawingUtil.DrawGrid(drawingContext, gridPen, Width, Height);

                if (DrawPath && path != null)
                {
                    var pathPen = new Pen(new SolidColorBrush(PathColor), 0.1D);
                    pathPen.Freeze();

                    var geometry = new StreamGeometry();
                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        var points = path.Points.Select(p => new Point(p.J + 0.5, p.I + 0.5));
                        ctx.BeginFigure(points.First(), true, false);
                        ctx.PolyLineTo(points.Skip(1).ToList(), true, true);
                    }
                    geometry.Freeze();

                    drawingContext.DrawGeometry(null, pathPen, geometry);
                }

                GridIndex startLoc = m_StartLocation;
                if(DrawStartPoint)
                    drawingContext.DrawEllipse(new SolidColorBrush(StartPointColor), null, new Point(startLoc.J + 0.5, startLoc.I + 0.5), 0.3, 0.3);
            }
            return new DrawingImage(visual.Drawing);
        }

        public ImageSource GenerateTableTexture(EdgeData data)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(BackgroundColor), null, new Rect(new Size(Width, Height)));
                
                var gridPen = new Pen(new SolidColorBrush(GridColor), 0.01D);
                DrawingUtil.DrawGrid(drawingContext, gridPen, Width, Height);

                foreach (var idx in data.Bounds.EnumerateEdge())
                {
                    DrawingUtil.DrawTableCell(drawingContext, idx.J, idx.I, FormattingUtil.FormatShort(data[idx]), ForegroundColor);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
