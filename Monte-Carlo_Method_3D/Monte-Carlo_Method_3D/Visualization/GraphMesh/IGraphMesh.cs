using System.Windows.Media.Media3D;
using JetBrains.Annotations;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Visualization.GraphMesh
{
    public interface IGraphMesh
    {
        MeshGeometry3D Mesh { get; }

        void UpdateMesh([NotNull] GridData data);
    }
}