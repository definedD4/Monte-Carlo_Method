using System;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public abstract class StGridVisualContext : StVisualContext, IGridContext
    {
        public StGridVisualContext(StSimulator simulator, StVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public double GetValueAtImageCoordinates(Point relativePos)
        {
            GridIndex idx = new GridIndex(
                (int)Math.Truncate(relativePos.Y * Visualizer.Height),
                (int)Math.Truncate(relativePos.X * Visualizer.Width)
            );
            var data = Simulator.GetData();
            return data.CanIndex(idx) ? data[idx] : double.NaN;
        }

        public override void UpdateVisualization()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> DataChanged;
    }
}
