using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class PrVisualContext3D : PrVisualContext, IModelRender
    {
        private GeometryModel3D m_Model;

        public PrVisualContext3D(PrSimulator simulator, PrVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public GeometryModel3D Model
        {
            get { return m_Model; }
            set { m_Model = value; OnPropertyChanged(nameof(Model)); }
        }

        public override void UpdateVisualization()
        {
            Model = Visualizer.GenerateModel(Simulator.GetData());
        }

        public double ModelScale => Math.Min(100d / Math.Max(Visualizer.Width, Visualizer.Height), 1d);

    }
}
