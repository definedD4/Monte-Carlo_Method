using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.ComponentModel;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class VisualContext : INotifyPropertyChanged
    {
        public PropabilityMethodSimulator Simulator { get; set; }
        public PropabilityMethodVisualizer Visualizer { get; set; }
        
        public VisualContext(PropabilityMethodSimulator simulator, PropabilityMethodVisualizer visualizer)
        {
            Simulator = simulator;
            Visualizer = visualizer;
        }

        public virtual void Update()
        {

        }

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
