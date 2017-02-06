using System.Globalization;
using System.Linq;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.VisualizationModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;
using Monte_Carlo_Method_3D.AppSettings;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class Visualizer2D
    {
        private const int Dpi = 96;

        public Visualizer2D()
        {
        }

        public GridTableVisualization GenerateTableVisualization([NotNull] GridData data)
        {
            var options = Settings.Current.VisualizationOptions;

            var backgroundBrush = new SolidColorBrush(options.BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (options.DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, options.GridPen, data.Size.Width, data.Size.Height);
                }

                // Draw values
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    var formattedText = new FormattedText(FormattingUtil.FormatShort(data[idx]),
                        CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        options.TextTypeface, options.TextEmSize, options.ForegroundBrush) {TextAlignment = TextAlignment.Center};
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
            var options = Settings.Current.VisualizationOptions;
            var backgroundBrush = new SolidColorBrush(options.BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw colors
                drawingContext.DrawImage(GenerateGridColorImage(data), new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (options.DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, options.GridPen, data.Size.Width, data.Size.Height);
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new GridTableVisualization(image, data);
        }

        public EdgeTableVisualization GenerateEdgeTableVisualization([NotNull] EdgeData data)
        {
            var options = Settings.Current.VisualizationOptions;

            var backgroundBrush = new SolidColorBrush(options.BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (options.DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, options.GridPen, data.Size.Width, data.Size.Height);
                }

                // Draw values
                foreach (var idx in data.Bounds.EnumerateEdge())
                {
                    var formattedText = new FormattedText(FormattingUtil.FormatShort(data[idx]),
                        CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        options.TextTypeface, options.TextEmSize, options.ForegroundBrush)
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
            var options = Settings.Current.VisualizationOptions;

            var backgroundBrush = new SolidColorBrush(options.BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw colors
                drawingContext.DrawImage(GenerateEdgeColorImage(data), new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (options.DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, options.GridPen, data.Size.Width, data.Size.Height);
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new EdgeTableVisualization(image, data);
        }

        public EdgeTableVisualization GenerateEdgeColorVisualizationWithPath([NotNull] EdgeData data, [NotNull] StParticlePath path)
        {
            var options = Settings.Current.VisualizationOptions;

            var backgroundBrush = new SolidColorBrush(options.BackgroundColor);

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                // Draw background
                drawingContext.DrawRectangle(backgroundBrush, null, new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw colors
                drawingContext.DrawImage(GenerateEdgeColorImage(data), new Rect(0, 0, data.Size.Width, data.Size.Height));

                // Draw grid
                if (options.DrawGrid)
                {
                    DrawingUtil.DrawGrid(drawingContext, options.GridPen, data.Size.Width, data.Size.Height);
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
                    drawingContext.DrawGeometry(null, options.PathPen, pathGeometry);
                }

                // Draw start point
                if (options.DrawStartPoint)
                {
                    drawingContext.PushTransform(new TranslateTransform(path.StartPoint.J + 0.5, path.StartPoint.I + 0.5));
                    drawingContext.DrawDrawing(options.StartPointDrawing);
                    drawingContext.Pop();
                }

                // Draw end point
                if (options.DrawEndPoint && path.EndPoint.HasValue)
                {
                    var pnt = path.EndPoint.GetValueOrDefault();
                    drawingContext.PushTransform(new TranslateTransform(pnt.J + 0.5, pnt.I + 0.5));
                    drawingContext.DrawDrawing(options.EndPointDrawing);
                    drawingContext.Pop();
                }
            }

            var image = new DrawingImage(visual.Drawing);
            image.Freeze();
            return new EdgeTableVisualization(image, data);
        }

        private ImageSource GenerateGridColorImage([NotNull] GridData data)
        {
            var options = Settings.Current.VisualizationOptions;

            var bitmap = new WriteableBitmap(data.Size.Width, data.Size.Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.BackBufferStride;

            bitmap.Lock();
            unsafe
            {
                byte* bytes = (byte*) bitmap.BackBuffer.ToPointer();
                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I, options.Pallete.GetColor(data[idx]), bytesPerPixel, stride);
                }
            }
            bitmap.Unlock();
            bitmap.Freeze();
            return bitmap;
        }

        private ImageSource GenerateEdgeColorImage([NotNull] EdgeData data)
        {
            var options = Settings.Current.VisualizationOptions;

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
                        data.CanIndex(idx) ? options.Pallete.GetColor(data[idx]) : options.BackgroundColor,
                        bytesPerPixel, stride);
                }
            }
            bitmap.Unlock();
            bitmap.Freeze();
            return bitmap;
        }
    }
}
