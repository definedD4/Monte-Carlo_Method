using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Visualization;
using System;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class StVisualContext : ViewModelBase
    {
        protected StVisualContext(StSimulator simulator, StVisualizer visualizer)
        {
            if (simulator == null)
                throw new ArgumentException();

            if (visualizer == null)
                throw new ArgumentException();

            Simulator = simulator;
            Visualizer = visualizer;
        }

        public StSimulator Simulator { get; set; }

        public StVisualizer Visualizer { get; set; }

        public abstract void UpdateVisualization();
    }
}