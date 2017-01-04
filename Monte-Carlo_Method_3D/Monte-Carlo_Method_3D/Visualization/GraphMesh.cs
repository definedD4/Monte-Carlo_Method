using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class GraphMesh
    {
        public GraphMesh(int width, int height)
        {
            Width = width;
            Height = height;

            Mesh = new MeshGeometry3D();
            InitMesh();
        } 

        public int Width { get; }
        public int Height { get; }
        public MeshGeometry3D Mesh { get; private set; }

        /// <summary>
        /// Converts specified indexed point in graph to position in 3d model.
        /// </summary>
        /// <param name="x">X index of the point in the graph</param>
        /// <param name="y">Y index of the point in the graph</param>
        /// <param name="value">Optional value of specified point, sets's the elevation of 3d point. 0 by default.</param>
        /// <returns></returns>
        public Point3D GetPoint(int x, int y, double value = 0d)
        {
            return new Point3D((double)x / (Width - 1) - 0.5,
                               value - 0.5,
                               (double)y / (Height - 1) - 0.5);
        }

        public GridIndex GetIndex(Point3D point)
        {
            return new GridIndex((int)((point.X + 0.5) * (Width - 1)), (int)((point.Z + 0.5) * (Height - 1)));
        }

        private void InitMesh()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Mesh.Positions.Add(GetPoint(x, y));
                    Mesh.TextureCoordinates.Add(new System.Windows.Point((double)x / (Width - 1), (double)y / (Height - 1)));
                }
            }

            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int i = x * Height + y;
                    Mesh.TriangleIndices.Add(i);
                    Mesh.TriangleIndices.Add(i + 1);
                    Mesh.TriangleIndices.Add(i + Height);

                    Mesh.TriangleIndices.Add(i + Height);
                    Mesh.TriangleIndices.Add(i + 1);
                    Mesh.TriangleIndices.Add(i + Height + 1);
                }
            }
        }

        public void UpdateMesh(GridData data)
        {
            if (data.Size != new GridSize(Height, Width))
                throw new ArgumentException("Data dimensions don't match up.");

            for(int i = 0; i < Mesh.Positions.Count; i++)
            {
                Point3D p = Mesh.Positions[i];
                GridIndex index = GetIndex(p);

                double val = data[index];
                p.Y = Math.Sqrt(val);

                Mesh.Positions[i] = p;
            }
        }
    }
}
