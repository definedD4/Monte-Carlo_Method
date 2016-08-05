using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class StVisualContext : ViewModelBase
    {
        private StSimulator m_Simulator;
        private StVisualizer m_Visualizer;

        protected StVisualContext(StSimulator simulator, StVisualizer visualizer)
        {
            m_Simulator = simulator;
            m_Visualizer = visualizer;
        }

        public StSimulator Simulator
        {
            get { return m_Simulator; }
            set { m_Simulator = value; OnPropertyChanged(nameof(Simulator));}
        }

        public StVisualizer Visualizer
        {
            get { return m_Visualizer; }
            set { m_Visualizer = value; OnPropertyChanged(nameof(Visualizer));}
        }

        public abstract void UpdateVisualization();
    }
}