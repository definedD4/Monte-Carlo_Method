using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class DiffVisualizer
    {
        private DiffGenerator m_Generator;

        public DiffVisualizer(DiffGenerator generator)
        {
            m_Generator = generator;
        }

        public int Width => m_Generator.Width;
        public int Height => m_Generator.Height;

        public Color BackgroundColor { get; set; } = Colors.White;
        public Color ForegroundColor { get; set; } = Colors.Black;

        public ImageSource GenerateTableTexture()
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(BackgroundColor), null, new Rect(new Size(Width, Height)));
                for (int x = 0; x < Width; x++)
                {
                    DrawingUtil.DrawTableCell(drawingContext, x, 0, Math.Round(m_Generator[x, 0], 5).ToString("E2"), ForegroundColor);
                    DrawingUtil.DrawTableCell(drawingContext, x, Height - 1, Math.Round(m_Generator[x, Height - 1], 5).ToString("E2"), ForegroundColor);
                }
                for (int y = 0; y < Height; y++)
                {
                    DrawingUtil.DrawTableCell(drawingContext, 0, y, Math.Round(m_Generator[0, y], 5).ToString("E2"), ForegroundColor);
                    DrawingUtil.DrawTableCell(drawingContext, Width - 1, y, Math.Round(m_Generator[Width - 1, y], 5).ToString("E2"), ForegroundColor);
                }
            }
            var image = new DrawingImage(visual.Drawing);
            return image;
        }
    }
}
