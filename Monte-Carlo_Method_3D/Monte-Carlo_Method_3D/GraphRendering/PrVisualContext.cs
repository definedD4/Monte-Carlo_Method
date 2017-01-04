using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.ComponentModel;
using Monte_Carlo_Method_3D.ViewModels;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class PrVisualContext : ViewModelBase
    {
        public PrSimulator Simulator { get; set; }
        public PrVisualizer Visualizer { get; set; }
        
        public PrVisualContext(PrSimulator simulator, PrVisualizer visualizer)
        {
            if (simulator == null)
                throw new ArgumentException();

            if (visualizer == null)
                throw new ArgumentException();

            Simulator = simulator;
            Visualizer = visualizer;
        }

        public abstract void UpdateVisualization();
    }
}
