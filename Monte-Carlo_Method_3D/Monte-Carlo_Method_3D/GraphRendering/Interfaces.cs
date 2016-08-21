using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public interface IGridContext
    {
        double GetValueAtImageCoordinates(Point position, Size controlSize);
    }

    public interface ITextureRender
    {
        ImageSource Texture { get; }
    }

    interface ITableRender
    {
        ImageSource Texture { get; }
    }

    interface IModelRender
    {
        GeometryModel3D Model { get; } 
    }
}
