using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class StVisualContext2D : StGridVisualContext
    {
        public StVisualContext2D(StSimulator simulator, StVisualizer visualizer)
            : base(simulator, visualizer)
        {
        }

        public virtual ImageSource Texture => Visualizer.Texture;

        public override void UpdateVisualization()
        {
            Visualizer.UpdateTexture(Simulator);
            OnPropertyChanged(nameof(Texture));
        }
    }
}