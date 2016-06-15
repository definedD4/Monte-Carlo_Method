using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class TableVisualContext : VisualContext2D
    {
        public TableVisualContext(PropabilityMethodSimulator simulator, PropabilityMethodVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public override ImageSource Texture => Visualizer.TableTexture;

        public override void Update()
        {
            Visualizer.UpdateTableTexture(Simulator);
            OnPropertyChanged("Texture");
        }
    }
}
