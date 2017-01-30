using System.Globalization;
using System.Linq;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.VisualizationModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class Visualizer2D
    {
        private const int Dpi = 96;

        public Pallete Pallete { get; set; } = new Pallete();
        public Color BackgroundColor { get; set; } = Colors.White;

        public Brush ForegroundBrush { get; set; } = new SolidColorBrush(Colors.Black);
        public Typeface TextTypeface { get; set; } = new Typeface("Segoe UI");
        public double TextEmSize { get; set; } = 0.15;

        public bool DrawGrid { get; set; } = true;
        public Pen GridPen { get; set; } = new Pen(new SolidColorBrush(Colors.DarkGray), 0.01D);

        public Pen PathPen { get; set; } = new Pen(new SolidColorBrush(Colors.Black), 0.1D);

        public bool DrawStartPoint { get; set; } = true;

        public Drawing StartPointDrawing { get; set; } = new GeometryDrawing(new SolidColorBrush(Colors.DarkBlue),
            null, 
            new EllipseGeometry(new Point(0, 0), 0.2, 0.2));

        public bool DrawEndPoint { get; set; } = true;

        public Drawing EndPointDrawing { get; set; } = new GeometryDrawing(new SolidColorBrush(Colors.Brown),
            null,
            new EllipseGeometry(new Point(0, 0), 0.2, 0.2));

        public Visualizer2D()
        {
            
        }

        public GridTableVisualization GenerateTableVisualization([NotNull] GridData data)
        {
            var backgroundBrush = new SolidColorBrush(BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, GridPen, data.Size.Width, data.Size.Height);
                }

                // Draw values
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    var formattedText = new FormattedText(FormattingUtil.FormatShort(data[idx]),
                        CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        TextTypeface, TextEmSize, ForegroundBrush) {TextAlignment = TextAlignment.Center};
                    drawingContext.DrawText(formattedText,
                            new Point(idx.J + 0.5f, idx.I + 0.5f - formattedText.Height / 2f));
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new GridTableVisualization(image, data);
        }

        public GridTableVisualization GenerateColorVisualization([NotNull] GridData data)
        {
            var backgroundBrush = new SolidColorBrush(BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw colors
                drawingContext.DrawImage(GenerateGridColorImage(data), new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, GridPen, data.Size.Width, data.Size.Height);
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new GridTableVisualization(image, data);
        }

        public EdgeTableVisualization GenerateEdgeTableVisualization([NotNull] EdgeData data)
        {
            var backgroundBrush = new SolidColorBrush(BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, GridPen, data.Size.Width, data.Size.Height);
                }

                // Draw values
                foreach (var idx in data.Bounds.EnumerateEdge())
                {
                    var formattedText = new FormattedText(FormattingUtil.FormatShort(data[idx]),
                        CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        TextTypeface, TextEmSize, ForegroundBrush)
                    { TextAlignment = TextAlignment.Center };
                    drawingContext.DrawText(formattedText,
                            new Point(idx.J + 0.5f, idx.I + 0.5f - formattedText.Height / 2f));
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new EdgeTableVisualization(image, data);
        }

        public EdgeTableVisualization GenerateEdgeColorVisualization([NotNull] EdgeData data)
        {
            var backgroundBrush = new SolidColorBrush(BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw colors
                drawingContext.DrawImage(GenerateEdgeColorImage(data), new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, GridPen, data.Size.Width, data.Size.Height);
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new EdgeTableVisualization(image, data);
        }

        public EdgeTableVisualization GenerateEdgeColorVisualizationWithPath([NotNull] EdgeData data, [NotNull] StParticlePath path)
        {
            var backgroundBrush = new SolidColorBrush(BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw colors
                drawingContext.DrawImage(GenerateEdgeColorImage(data), new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, GridPen, data.Size.Width, data.Size.Height);
                }

                // Draw path
                if (path.Points.Count > 0)
                {
                    var pathGeometry = new StreamGeometry();
                    using (StreamGeometryContext ctx = pathGeometry.Open())
                    {
                        var points = path.Points.Select(p => new Point(p.J + 0.5, p.I + 0.5)).ToList();
                        ctx.BeginFigure(points.First(), true, false);
                        ctx.PolyLineTo(points.Skip(1).ToList(), true, true);
                    }
                    pathGeometry.Freeze();
                    drawingContext.DrawGeometry(null, PathPen, pathGeometry);
                }

                // Draw start point
                if (DrawStartPoint)
                {
                    drawingContext.PushTransform(new TranslateTransform(path.StartPoint.J + 0.5, path.StartPoint.I + 0.5));
                    drawingContext.DrawDrawing(StartPointDrawing);
                    drawingContext.Pop();
                }

                // Draw end point
                if (DrawEndPoint && path.EndPoint.HasValue)
                {
                    var pnt = path.EndPoint.GetValueOrDefault();
                    drawingContext.PushTransform(new TranslateTransform(pnt.J + 0.5, pnt.I + 0.5));
                    drawingContext.DrawDrawing(EndPointDrawing);
                    drawingContext.Pop();
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new EdgeTableVisualization(image, data);
        }

        private ImageSource GenerateGridColorImage([NotNull] GridData data)
        {
            var bitmap = new WriteableBitmap(data.Size.Width, data.Size.Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.BackBufferStride;

            bitmap.Lock();
            unsafe
            {
                byte* bytes = (byte*) bitmap.BackBuffer.ToPointer();
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I, Pallete.GetColor(data[idx]), bytesPerPixel, stride);
                }
            }
            bitmap.Unlock();
            bitmap.Freeze();
            return bitmap;
        }

        private ImageSource GenerateEdgeColorImage([NotNull] EdgeData data)
        {
            var bitmap = new WriteableBitmap(data.Size.Width, data.Size.Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.BackBufferStride;

            bitmap.Lock();
            unsafe
            {
                byte* bytes = (byte*)bitmap.BackBuffer.ToPointer();
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I,
                        data.CanIndex(idx) ? Pallete.GetColor(data[idx]) : BackgroundColor,
                        bytesPerPixel, stride);
                }
            }
            bitmap.Unlock();
            bitmap.Freeze();
            return bitmap;
        }
    }
}
