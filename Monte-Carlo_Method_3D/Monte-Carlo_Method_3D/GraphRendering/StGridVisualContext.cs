using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class StGridVisualContext : StVisualContext, IGridContext
    {
        private double m_PointedValue;

        public StGridVisualContext(StSimulator simulator, StVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public double PointedValue
        {
            get { return m_PointedValue; }
            set { m_PointedValue = Math.Round(value, 8); OnPropertyChanged("PointedValue"); }
        }

        public double GetValueAtImageCoordinates(Point position, Size controlSize)
        {
            int x = (int)Math.Truncate(position.X * Visualizer.ImageWidth / controlSize.Width / Visualizer.PixelsPerCell);
            int y = (int)Math.Truncate(position.Y * Visualizer.ImageHeight / controlSize.Height / Visualizer.PixelsPerCell);

            if(Simulator == null)
                return double.NaN;

            if (x == 0 || x == Simulator.Width - 1 || y == 0 || y == Simulator.Height - 1)
            {
                return Simulator[x, y];
            }
            return double.NaN;
        }
    }
}
