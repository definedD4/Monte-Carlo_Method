using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StParticlePath
    {
        public StParticlePath(IList<IntPoint> points)
        {
            Points = points;
        }

        public IList<IntPoint> Points { get; private set; }
    }
}
