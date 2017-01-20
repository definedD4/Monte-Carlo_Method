using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class PrVisualizer
    {
        private const int Dpi = 96;

        private readonly Pallete m_Pallete;
        private readonly GraphMesh m_Mesh;

        public PrVisualizer(GridSize size, Pallete pallete)
        {
            Size = size;
            Width = Size.Columns;
            Height = Size.Rows;
            m_Pallete = pallete;

            m_Mesh = new GraphMesh(Size);     
        }

        public GridSize Size { get; }
        public int Width { get; }
        public int Height { get; }

        public Color BackgroundColor { get; set; } = Colors.White;
        public Color ForegroundColor { get; set; } = Colors.Black;
        public Color GridColor { get; set; } = Colors.DarkGray;

        public GeometryModel3D GenerateModel(GridData data)
        {
            if (data.Size != Size)
                throw new ArgumentException("Data has wrong dimensions.");

            m_Mesh.UpdateMesh(data);
            return new GeometryModel3D(m_Mesh.Mesh, new DiffuseMaterial(new ImageBrush(GenerateModelTexture(data))));
        }

        public ImageSource GenerateColorTexture(GridData data)
        {
            if (data.Size != Size)
                throw new ArgumentException("Data has wrong dimensions.");

            var image = GenerateModelTexture(data);
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(image, new Rect(0, 0, Width, Height));
                var gridPen = new Pen(new SolidColorBrush(GridColor), 0.01D);
                DrawingUtil.DrawGrid(drawingContext, gridPen, Width, Height);
            }
            return new DrawingImage(visual.Drawing);
        }

        private ImageSource GenerateModelTexture(GridData data)
        {
            WriteableBitmap bitmap = new WriteableBitmap(Width, Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.BackBufferStride;

            bitmap.Lock();
            unsafe
            {
                byte* bytes = (byte*)bitmap.BackBuffer.ToPointer();

                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    Color color = m_Pallete.GetColor(data[idx]);
                    DrawingUtil.DrawColorCell(bytes, idx.J, idx.I, color, bytesPerPixel, stride);
                }
                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                bitmap.Unlock();
            }
            bitmap.Freeze();

            return bitmap;
        }

        public ImageSource GenerateTableTexture(GridData data)
        {
            if (data.Size != Size)
                throw new ArgumentException("Data has wrong dimensions.");

            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(BackgroundColor), null, new Rect(new Size(Width, Height)));

                var gridPen = new Pen(new SolidColorBrush(GridColor), 0.01D);
                DrawingUtil.DrawGrid(drawingContext, gridPen, Width, Height);

                foreach (var idx in data.Bounds.EnumerateRegion())
                {
                    DrawingUtil.DrawTableCell(drawingContext, idx.J, idx.I, FormattingUtil.FormatShort(data[idx]), ForegroundColor);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
