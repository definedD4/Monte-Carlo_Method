using Monte_Carlo_Method_3D.Simulation;
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
        private readonly IPallete m_Pallete;
        private readonly PrSimulator m_Simulator;
        private MeshGeometry3D m_Mesh;

        public PrVisualizer(PrSimulator simulator, IPallete pallete)
        {
            m_Simulator = simulator;
            m_Pallete = pallete;

            m_Mesh = GenerateMesh(Width, Height);     
        }

        public int Width => m_Simulator.Width;
        public int Height => m_Simulator.Height;
        public int PixelsPerCell { get; set; } = 1;
        public int ImageWidth => Width * PixelsPerCell;
        public int ImageHeight => Height * PixelsPerCell;

        public double HeightCoefficient { get; set; }
        public bool DrawBorder { get; set; }

        public GeometryModel3D GenerateModel()
        {
            GeometryModel3D model = new GeometryModel3D(m_Mesh, new DiffuseMaterial(new ImageBrush(GenerateColorTexture())));

            double offsetX = -(double)m_Simulator.Width / 2;
            double offsetY = -(double)m_Simulator.Height / 2;

            Point3DCollection points = m_Mesh.Positions;
            for(int i = 0; i < points.Count; i++)
            {
                Point3D p = points[i];
                int x = (int)(p.X - offsetX);
                int y = (int)(p.Z - offsetY);
                p.Y = Math.Sqrt(Math.Sqrt(m_Simulator[x, y])) * HeightCoefficient;
                points[i] = p;
            }

            return model;
        }

        private MeshGeometry3D GenerateMesh(int width, int height)
        {
            double offsetX = -(double)width / 2;
            double offsetY = -(double)height / 2;

            MeshGeometry3D mesh = new MeshGeometry3D();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    mesh.Positions.Add(new Point3D(x + offsetX, 0, y + offsetY));
                    mesh.TextureCoordinates.Add(new Point((double)x / width, (double)y / height));
                }
            }

            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    int i = x * height + y;
                    mesh.TriangleIndices.Add(i);
                    mesh.TriangleIndices.Add(i + 1);
                    mesh.TriangleIndices.Add(i + height);

                    mesh.TriangleIndices.Add(i + height);
                    mesh.TriangleIndices.Add(i + 1);
                    mesh.TriangleIndices.Add(i + height + 1);
                }
            }
            return mesh;
        }

        public ImageSource GenerateColorTexture()
        {
            int cellSize = PixelsPerCell;
            int bytesPerPixel = 3;
            int dpi = 96;
            int width = m_Simulator.Width * cellSize;
            int height = m_Simulator.Height * cellSize;
            int stride = width * bytesPerPixel;
            byte[] bytes = new byte[height * stride];

            for (int x = 0; x < m_Simulator.Width; x++)
            {
                for (int y = 0; y < m_Simulator.Height; y++)
                {
                    for (int _x = 0; _x < cellSize; _x++)
                    {
                        for (int _y = 0; _y < cellSize; _y++)
                        {
                            Color color = Colors.Purple;

                            if(DrawBorder && (_x == 0 || _x == 9 || _y == 0 || _y == 9))
                            {
                                color = Colors.Black;
                            }
                            else
                            {
                                color = m_Pallete.GetColor(m_Simulator[x, y]);
                            }

                            int index = (y * cellSize + _y) * stride + (x * cellSize + _x) * bytesPerPixel;
                            bytes[index] = color.B;
                            bytes[index + 1] = color.G;
                            bytes[index + 2] = color.R;
                        }
                    }
                }
            }
            BitmapSource result = BitmapSource.Create(width, height, dpi, dpi, PixelFormats.Bgr24, null, bytes, stride);
            return result;
        }

        public ImageSource GenerateTableTexture()
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Black, null, new Rect(new Size(ImageWidth, ImageHeight)));
                for (int i = 0; i < m_Simulator.Width; i++)
                {
                    for (int j = 0; j < m_Simulator.Height; j++)
                    {
                        double val = m_Simulator[i, j];
                        drawingContext.DrawText(new FormattedText(Math.Round(val, 5).ToString("E2"), 
                            CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 0.15, Brushes.White),
                            new Point(i * PixelsPerCell + PixelsPerCell * 0.3f, j * PixelsPerCell + PixelsPerCell * 0.3f));
                    }
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
