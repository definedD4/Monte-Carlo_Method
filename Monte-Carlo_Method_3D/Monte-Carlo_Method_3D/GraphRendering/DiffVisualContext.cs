using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class DiffVisualContext : ViewModelBase,  IGridContext, ITextureRender
    {
        private ImageSource m_Texture;

        public DiffVisualContext(DiffGenerator generator, DiffVisualizer visualizer)
        {
            Generator = generator;
            Visualizer = visualizer;
        }

        public DiffGenerator Generator { get; set; }
        public DiffVisualizer Visualizer { get; set; }

        public double GetValueAtImageCoordinates(Point position, Size controlSize)
        {
            if (Generator == null)
                return double.NaN;

            int x = (int)Math.Truncate(position.X * Visualizer.Width / controlSize.Width);
            int y = (int)Math.Truncate(position.Y * Visualizer.Height / controlSize.Height);

            if (!Generator.CanIndex(x, y))
                return double.NaN;

            return Generator[x, y]; ;
        }

        public void UpdateVisualization()
        {
            Texture = Visualizer.GenerateTableTexture();
        }

        public ImageSource Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; OnPropertyChanged(nameof(Texture)); }
        }
    }
}
