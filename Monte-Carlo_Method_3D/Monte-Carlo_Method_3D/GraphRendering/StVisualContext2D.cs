using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class StVisualContext2D : StGridVisualContext
    {
        private ImageSource m_Texture;

        public StVisualContext2D(StSimulator simulator, StVisualizer visualizer)
            : base(simulator, visualizer)
        {
        }

        public virtual ImageSource Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; OnPropertyChanged(nameof(Texture)); }
        }

        public override void UpdateVisualization()
        {
            Texture = Visualizer.GenerateColorTexture(true);
        }
    }
}