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
            if(Simulator == null)
                return double.NaN;

            int x = (int)Math.Truncate(position.X * Visualizer.Width / controlSize.Width);
            int y = (int)Math.Truncate(position.Y * Visualizer.Height / controlSize.Height);

            if(!Simulator.CanIndex(x, y))
                return double.NaN;

            return Simulator[x, y];
        }
    }
}
