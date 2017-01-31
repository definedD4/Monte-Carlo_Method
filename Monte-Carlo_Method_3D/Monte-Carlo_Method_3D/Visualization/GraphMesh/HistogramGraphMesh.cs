using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Visualization.GraphMesh
{
    public class HistogramGraphMesh : IGraphMesh
    {
        public HistogramGraphMesh(GridSize size)
        {
            Size = size;
            Mesh = new MeshGeometry3D();

            InitMesh();
        }

        public GridSize Size { get; }

        public MeshGeometry3D Mesh { get; private set; }

        private void InitMesh()
        {
            foreach (var idx in new GridRegion(GridIndex.Zero, Size).EnumerateRegion())
            {
                AddHist(idx);   
            }
        }

        private void AddHist(GridIndex index)
        {
            int idx = index.Offset(Size) * 8;

            double x1 = (double) index.J      / (Size.Width  - 1) - 0.5d;
            double x2 = (double)(index.J + 1) / (Size.Width  - 1) - 0.5d;
            double y1 = (double) index.I      / (Size.Height - 1) - 0.5d;
            double y2 = (double)(index.I + 1) / (Size.Height - 1) - 0.5d;

            double tX1 = (index.J + 0.4d) / (Size.Width - 1);
            double tX2 = (index.J + 0.6d) / (Size.Width - 1);
            double tY1 = (index.I + 0.4d) / (Size.Height - 1);
            double tY2 = (index.I + 0.6d) / (Size.Height - 1);

            // Upper
            Mesh.Positions.Add(new Point3D(x1, 0, y1));
            Mesh.Positions.Add(new Point3D(x2, 0, y1));
            Mesh.Positions.Add(new Point3D(x2, 0, y2));
            Mesh.Positions.Add(new Point3D(x1, 0, y2));

            Mesh.TextureCoordinates.Add(new Point(tX1, tY1));
            Mesh.TextureCoordinates.Add(new Point(tX2, tY1));
            Mesh.TextureCoordinates.Add(new Point(tX2, tY2));
            Mesh.TextureCoordinates.Add(new Point(tX1, tY2));

            /*Mesh.TextureCoordinates.Add(new Point((double)index.J / Size.Width, (double)index.I / Size.Height));
            Mesh.TextureCoordinates.Add(new Point((double)(index.J + 1) / Size.Width, (double)index.I / Size.Height));
            Mesh.TextureCoordinates.Add(new Point((double)(index.J + 1) / Size.Width, (double)(index.I + 1) / Size.Height));
            Mesh.TextureCoordinates.Add(new Point((double)index.J / Size.Width, (double)(index.I + 1) / Size.Height));*/

            // Lower
            Mesh.Positions.Add(new Point3D(x1, 0, y1));
            Mesh.Positions.Add(new Point3D(x2, 0, y1));
            Mesh.Positions.Add(new Point3D(x2, 0, y2));
            Mesh.Positions.Add(new Point3D(x1, 0, y2));

            Mesh.TextureCoordinates.Add(new Point(tX1, tY1));
            Mesh.TextureCoordinates.Add(new Point(tX2, tY1));
            Mesh.TextureCoordinates.Add(new Point(tX2, tY2));
            Mesh.TextureCoordinates.Add(new Point(tX1, tY2));
            /*Mesh.TextureCoordinates.Add(new Point((double)index.J / Size.Width, (double)index.I / Size.Height));
            Mesh.TextureCoordinates.Add(new Point((double)(index.J + 1) / Size.Width, (double)index.I / Size.Height));
            Mesh.TextureCoordinates.Add(new Point((double)(index.J + 1) / Size.Width, (double)(index.I + 1) / Size.Height));
            Mesh.TextureCoordinates.Add(new Point((double)index.J / Size.Width, (double)(index.I + 1) / Size.Height));*/

            AddFace(idx + 0, idx + 1, idx + 2, idx + 3);
            AddFace(idx + 1, idx + 0, idx + 4, idx + 5);
            AddFace(idx + 2, idx + 1, idx + 5, idx + 6);
            AddFace(idx + 3, idx + 2, idx + 6, idx + 7);
            AddFace(idx + 0, idx + 3, idx + 7, idx + 4);
        }

        private void AddFace(int v1, int v2, int v3, int v4)
        {
            Mesh.TriangleIndices.Add(v2);
            Mesh.TriangleIndices.Add(v1);
            Mesh.TriangleIndices.Add(v3);
            Mesh.TriangleIndices.Add(v3);
            Mesh.TriangleIndices.Add(v1);
            Mesh.TriangleIndices.Add(v4);
        }
        
        public void UpdateMesh(GridData data)
        {
            foreach (var idx in data.Bounds.EnumerateRegion())
            {
                double val = Math.Pow(data[idx], 1 / 8d);

                int i = idx.Offset(Size) * 8;

                Point3D p;

                p = Mesh.Positions[i    ]; p.Y = val; Mesh.Positions[i    ] = p;
                p = Mesh.Positions[i + 1]; p.Y = val; Mesh.Positions[i + 1] = p;
                p = Mesh.Positions[i + 2]; p.Y = val; Mesh.Positions[i + 2] = p;
                p = Mesh.Positions[i + 3]; p.Y = val; Mesh.Positions[i + 3] = p;
            }
        }
    }
}