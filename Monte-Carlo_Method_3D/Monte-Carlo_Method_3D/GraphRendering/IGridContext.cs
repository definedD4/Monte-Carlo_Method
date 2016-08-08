using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public interface IGridContext
    {
        double PointedValue { get; set; }

        double GetValueAtImageCoordinates(Point position, Size controlSize);
    }
}
