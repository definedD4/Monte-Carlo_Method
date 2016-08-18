using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class PrGridVisualContext : PrVisualContext, IGridContext
    {
        private double m_PointedValue;

        public PrGridVisualContext(PrSimulator simulator, PrVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public double GetValueAtImageCoordinates(Point position, Size controlSize)
        {
            int x = (int)Math.Truncate(position.X * Visualizer.Width / controlSize.Width);
            int y = (int)Math.Truncate(position.Y * Visualizer.Height / controlSize.Height);
            return Simulator != null ? Simulator[x, y] : double.NaN;
        }
    }
}
