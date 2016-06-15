using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class VisualContext3D : VisualContext
    {
        public VisualContext3D(PropabilityMethodSimulator simulator, PropabilityMethodVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public GeometryModel3D Model => Visualizer.Model;

        public override void Update()
        {
            base.Update();
            Visualizer.UpdateModelAndTexture(Simulator);
            OnPropertyChanged("Model");
        }

        public double ModelScale => Math.Min(100d / Math.Max(Visualizer.Width, Visualizer.Height), 1d);

    }
}
