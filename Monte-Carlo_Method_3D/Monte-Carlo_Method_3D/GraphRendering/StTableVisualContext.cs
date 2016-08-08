using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class StTableVisualContext : StGridVisualContext
    {
        private ImageSource m_Texture;

        public StTableVisualContext(StSimulator simulator, StVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public ImageSource Texture
        {
            get { return m_Texture; }
            private set { m_Texture = value; OnPropertyChanged(nameof(Texture)); }
        }

        public override void UpdateVisualization()
        {
            Texture = Visualizer.GenerateTableTexture();
            OnPropertyChanged("Texture");
        }
    }
}
