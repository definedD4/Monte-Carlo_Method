using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class VisualContext2D : GridVisualContext
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
    }
}
