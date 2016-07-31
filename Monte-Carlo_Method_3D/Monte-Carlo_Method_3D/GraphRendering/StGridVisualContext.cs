using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class StGridVisualContext : StVisualContext
    {
        public StGridVisualContext(StatMethodSimulator simulator, StatMethodVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        private double p_PointedValue;
        public double PointedValue
        {
            get { return p_PointedValue; }
            set { p_PointedValue = Math.Round(value, 8); OnPropertyChanged("PointedValue"); }
        }

        public double GetValueAtImageCoordinates(Point position, Size controlSize)
        {
            int x = (int)Math.Truncate(position.X * Visualizer.ImageWidth / controlSize.Width / Visualizer.PixelsPerCell);
            int y = (int)Math.Truncate(position.Y * Visualizer.ImageHeight / controlSize.Height / Visualizer.PixelsPerCell);
            return Simulator != null ? Simulator[x, y] : double.NaN;
        }
    }
}
