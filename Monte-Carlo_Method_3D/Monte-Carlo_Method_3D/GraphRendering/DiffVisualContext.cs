using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class DiffVisualContext : ViewModelBase,  IGridContext, ITextureRender
    {
        private ImageSource m_Texture;

        public DiffVisualContext(DiffGenerator generator, DiffVisualizer visualizer)
        {
            if (generator == null)
                throw new ArgumentException();

            if (visualizer == null)
                throw new ArgumentException();

            Generator = generator;
            Visualizer = visualizer;
        }

        public DiffGenerator Generator { get; set; }
        public DiffVisualizer Visualizer { get; set; }

        public double GetValueAtImageCoordinates(Point relativePos)
        {
            GridIndex idx = new GridIndex(
                (int)Math.Truncate(relativePos.X * Visualizer.Width),
                (int)Math.Truncate(relativePos.Y * Visualizer.Height)
            );
            var data = Generator.GetData();
            return data.CanIndex(idx) ? data[idx] : double.NaN;
        }

        public void UpdateVisualization()
        {
            Texture = Visualizer.GenerateTableTexture(Generator.GetData());
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        public ImageSource Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; OnPropertyChanged(nameof(Texture)); }
        }

        public event EventHandler<EventArgs> DataChanged;
    }
}
