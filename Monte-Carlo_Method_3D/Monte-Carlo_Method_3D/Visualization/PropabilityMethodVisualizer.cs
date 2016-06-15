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
    public class PropabilityMethodVisualizer
    {
        private IPallete pallete;

        public PropabilityMethodVisualizer(int width, int height, IPallete pallete)
        {
            Width = width;
            Height = height;
            this.pallete = pallete;
            mesh = GenerateMesh(Width, Height);
            Brush brush = new ImageBrush(Texture);
            Material material = new DiffuseMaterial(brush);
            Model = new GeometryModel3D(mesh, material);
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int PixelsPerCell { get; set; } = 3;
        public int ImageWidth => Width * PixelsPerCell;
        public int ImageHeight => Height * PixelsPerCell;

        private MeshGeometry3D mesh;

        public GeometryModel3D Model { get; private set; }
        public ImageSource Texture { get; private set; }
        public ImageSource TableTexture { get; private set; }

        public double HeightCoefficient { get; set; }
        public bool DrawBorder { get; set; }

        public void UpdateModelAndTexture(PropabilityMethodSimulator simulator)
        {
            UpdateTexture(simulator);
            Model.Material = new DiffuseMaterial(new ImageBrush(Texture));

            Stopwatch s = Stopwatch.StartNew();
            double offsetX = -(double)simulator.Width / 2;
            double offsetY = -(double)simulator.Height / 2;

            Point3DCollection points = mesh.Positions;
            for(int i = 0; i < points.Count; i++)
            {
                Point3D p = points[i];
                int x = (int)(p.X - offsetX);
                int y = (int)(p.Z - offsetY);
                p.Y = Math.Sqrt(Math.Sqrt(simulator[x, y])) * HeightCoefficient;
                points[i] = p;
            }
            s.Stop();
        }

        public void UpdateTexture(PropabilityMethodSimulator simulator)
        {
            Texture = GenerateTexture(simulator);
        }

        public void UpdateTableTexture(PropabilityMethodSimulator simulator)
        {
            TableTexture = GenerateTableTexture(simulator);
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

        private ImageSource GenerateTexture(PropabilityMethodSimulator simulator)
        {
            int cellSize = PixelsPerCell;
            int bytesPerPixel = 3;
            int dpi = 96;
            int width = simulator.Width * cellSize;
            int height = simulator.Height * cellSize;
            int stride = width * bytesPerPixel;
            byte[] bytes = new byte[height * stride];

            for (int x = 0; x < simulator.Width; x++)
            {
                for (int y = 0; y < simulator.Height; y++)
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
                                color = pallete.GetColor(simulator[x, y]);
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

        private ImageSource GenerateTableTexture(PropabilityMethodSimulator simulator)
        {
            int cellSize = PixelsPerCell;
            int width = simulator.Width * cellSize;
            int height = simulator.Height * cellSize;
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Black, null, new Rect(new Size(width, height)));
                for (int i = 0; i < simulator.Width; i++)
                {
                    for (int j = 0; j < simulator.Height; j++)
                    {
                        double val = simulator[i, j];
                        drawingContext.DrawText(new FormattedText((val != 0) ? "1/" + Math.Round(1 / val, 1) : "0", 
                            CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 1, Brushes.White), new Point(i * cellSize, j * cellSize));
                    }
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
