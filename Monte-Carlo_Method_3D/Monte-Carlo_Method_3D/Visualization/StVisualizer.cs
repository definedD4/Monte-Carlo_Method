using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class StVisualizer
    {
        private IPallete m_Pallete;

        public StVisualizer(int width, int height, IPallete pallete)
        {
            m_Pallete = pallete;
            Width = width;
            Height = height;
        }

        public int PixelsPerCell { get; set; } = 1;
        public ImageSource Texture { get; private set; }
        public ImageSource TableTexture { get; private set; }
        public bool DrawBorder { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ImageWidth => Width * PixelsPerCell;
        public int ImageHeight => Height * PixelsPerCell;

        public void UpdateTexture(StSimulator simulator)
        {
            Texture = GenerateTexture(simulator);
        }

        public void UpdateTableTexture(StSimulator simulator)
        {
            TableTexture = GenerateTableTexture(simulator);
        }

        private ImageSource GenerateTexture(StSimulator simulator)
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
                DrawCell(simulator, cellSize, x, 0, stride, bytesPerPixel, bytes);
                DrawCell(simulator, cellSize, x, simulator.Height - 1, stride, bytesPerPixel, bytes);
            }
            for (int y = 1; y < simulator.Height; y++)
            {
                DrawCell(simulator, cellSize, 0, y, stride, bytesPerPixel, bytes);
                DrawCell(simulator, cellSize, simulator.Width - 1, y, stride, bytesPerPixel, bytes);
            }
            BitmapSource result = BitmapSource.Create(width, height, dpi, dpi, PixelFormats.Bgr24, null, bytes, stride);
            return result;
        }

        private void DrawCell(StSimulator simulator, int cellSize, int x, int y, int stride, int bytesPerPixel,
            byte[] bytes)
        {
            for (int _x = 0; _x < cellSize; _x++)
            {
                for (int _y = 0; _y < cellSize; _y++)
                {
                    Color color = Colors.Purple;

                    if (DrawBorder && (_x == 0 || _x == cellSize - 1 || _y == 0 || _y == cellSize - 1))
                    {
                        color = Colors.Black;
                    }
                    else
                    {
                        color = m_Pallete.GetColor(simulator[x, y]);
                    }

                    int index = (y*cellSize + _y)*stride + (x*cellSize + _x)*bytesPerPixel;
                    bytes[index] = color.B;
                    bytes[index + 1] = color.G;
                    bytes[index + 2] = color.R;
                }
            }
        }

        private ImageSource GenerateTableTexture(StSimulator simulator)
        {
            int cellSize = PixelsPerCell;
            int width = simulator.Width * cellSize;
            int height = simulator.Height * cellSize;
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Black, null, new Rect(new Size(width, height)));
                for (int x = 0; x < simulator.Width; x++)
                {
                    DrawTableCell(simulator, x, 0, drawingContext, cellSize);
                    DrawTableCell(simulator, x, simulator.Height - 1, drawingContext, cellSize);
                }
                for (int y = 0; y < simulator.Height; y++)
                {
                    DrawTableCell(simulator, 0, y, drawingContext, cellSize);
                    DrawTableCell(simulator, simulator.Width - 1, y, drawingContext, cellSize);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }

        private void DrawTableCell(StSimulator simulator, int i, int j, DrawingContext drawingContext, int cellSize)
        {
            double val = simulator[i, j];
            drawingContext.DrawText(new FormattedText(Math.Round(val, 5).ToString(),
                CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 0.5, Brushes.White),
                new Point(i*cellSize + cellSize*0.3f, j*cellSize + cellSize*0.3f));
        }
    }
}
