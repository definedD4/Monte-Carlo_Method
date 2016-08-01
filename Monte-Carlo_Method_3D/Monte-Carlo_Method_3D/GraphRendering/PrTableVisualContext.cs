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
    public class PrTableVisualContext : PrGridVisualContext
    {
        private ImageSource m_Texture;

        public PrTableVisualContext(PrSimulator simulator, PrVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public ImageSource Texture
        {
            get { return m_Texture; }
            private set { m_Texture = value; OnPropertyChanged(nameof(Texture)); }
        }

        public override void Update()
        {
            Texture = Visualizer.GenerateTableTexture();
            OnPropertyChanged("Texture");
        }
    }
}
