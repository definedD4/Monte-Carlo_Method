using System.Collections.Generic;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class StParticlePath
    {
        public GridIndex StartPoint { get; }
        public GridIndex? EndPoint { get; }
        public IList<GridIndex> Points { get; }

        public StParticlePath(GridIndex startPoint, GridIndex? endPoint, IList<GridIndex> points)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Points = points;
        }
    }
}
