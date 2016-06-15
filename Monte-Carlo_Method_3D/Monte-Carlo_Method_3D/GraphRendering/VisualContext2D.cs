using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class VisualContext2D : VisualContext
    {
        public VisualContext2D(PropabilityMethodSimulator simulator, PropabilityMethodVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public virtual ImageSource Texture => Visualizer.Texture;

        public override void Update()
        {
            base.Update();
            Visualizer.UpdateTexture(Simulator);
            OnPropertyChanged("Texture");
        }

        private double p_PointedValue;
        public double PointedValue { get { return p_PointedValue; }
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
