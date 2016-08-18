using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class PrVisualizer
    {
        private const int Dpi = 96;

        private readonly Pallete m_Pallete;
        private readonly PrSimulator m_Simulator;
        private GraphMesh m_Mesh;

        public PrVisualizer(PrSimulator simulator, Pallete pallete)
        {
            m_Simulator = simulator;
            m_Pallete = pallete;

            m_Mesh = new GraphMesh(Width, Height);     
        }

        public int Width => m_Simulator.Width;
        public int Height => m_Simulator.Height;

        public Color BackgroundColor { get; set; } = Colors.White;
        public Color ForegroundColor { get; set; } = Colors.Black;

        public GeometryModel3D GenerateModel()
        {
            m_Mesh.UpdateMesh(m_Simulator);
            return new GeometryModel3D(m_Mesh.Mesh, new DiffuseMaterial(new ImageBrush(GenerateColorTexture())));
        }

        public ImageSource GenerateColorTexture()
        {
            WriteableBitmap bitmap = new WriteableBitmap(Width, Height, Dpi, Dpi, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.BackBufferStride;

            bitmap.Lock();
            unsafe
            {
                byte* bytes = (byte*)bitmap.BackBuffer.ToPointer();

                for (int x = 0; x < m_Simulator.Width; x++)
                {
                    for (int y = 0; y < m_Simulator.Height; y++)
                    {
                        Color color = m_Pallete.GetColor(m_Simulator[x, y]);

                        DrawingUtil.DrawColorCell(bytes, x, y, color, bytesPerPixel, stride);
                    }
                }
                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                bitmap.Unlock();
            }
            bitmap.Freeze();

            return bitmap;
        }

        public ImageSource GenerateTableTexture()
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(BackgroundColor), null, new Rect(new Size(Width, Height)));
                for (int i = 0; i < m_Simulator.Width; i++)
                {
                    for (int j = 0; j < m_Simulator.Height; j++)
                    {
                        DrawingUtil.DrawTableCell(drawingContext, i, j, Math.Round(m_Simulator[i, j], 5).ToString("E2"), ForegroundColor);
                    }
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
