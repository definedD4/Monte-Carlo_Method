using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class DiffVisualizer
    { 
        public DiffVisualizer(GridSize size)
        {
            Size = size;
            Width = Size.Columns;
            Height = Size.Rows;
        }

        public GridSize Size { get; }
        public int Width { get; }
        public int Height { get; }

        public Color BackgroundColor { get; set; } = Colors.White;
        public Color ForegroundColor { get; set; } = Colors.Black;
        public Color GridColor { get; set; } = Colors.DarkGray;

        public IVisualization GenerateTableVisualization(EdgeData data)
        {
            return new EdgeTableVisualization(GenerateTableTexture(data), data);
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
