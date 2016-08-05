using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.ComponentModel;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class PrVisualContext : INotifyPropertyChanged
    {
        public PrSimulator Simulator { get; set; }
        public PrVisualizer Visualizer { get; set; }
        
        public PrVisualContext(PrSimulator simulator, PrVisualizer visualizer)
        {
            Simulator = simulator;
            Visualizer = visualizer;
        }

        public abstract void UpdateVisualization();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Invalid property name.");

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
