using System;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Visualization.GraphMesh.Factory
{
    public class GraphMeshFactory
    {
        public static IGraphMesh Construct(GraphMeshKind kind, GridSize size)
        {
            switch (kind)
            {
                case GraphMeshKind.Triangluar:
                    return new TriangleGraphMesh(size);
                case GraphMeshKind.Histogram:
                    return new HistogramGraphMesh(size); ;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown graph mesh kind provided");
            }
        }
    }

    public enum GraphMeshKind
    {
        Triangluar,
        Histogram
    }
}