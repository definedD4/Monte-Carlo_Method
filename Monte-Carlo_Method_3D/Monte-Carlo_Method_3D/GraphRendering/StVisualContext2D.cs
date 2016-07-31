using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class StVisualContext2D : StGridVisualContext
    {
        public StVisualContext2D(StatMethodSimulator simulator, StatMethodVisualizer visualizer)
            : base(simulator, visualizer)
        {
        }

        public virtual ImageSource Texture => Visualizer.Texture;

        public override void UpdateVizualization()
        {
            Visualizer.UpdateTexture(Simulator);
            OnPropertyChanged(nameof(Texture));
        }
    }
}