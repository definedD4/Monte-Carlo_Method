using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.DataModel
{
    public class EdgeData
    {
        private readonly double[,] m_Data;    

        public GridSize Size { get; }
        public GridRegion Bounds { get; }
        public GridRegion Inaccessable { get; }

        public static EdgeData FromArray(double[,] data) => new EdgeData(new GridSize(data.GetLength(1), data.GetLength(0)), data);
        public static EdgeData AllocateNew(GridSize size) => new EdgeData(size, new double[size.Height, size.Width]);
        public static EdgeData AllocateLike(EdgeData other) => EdgeData.AllocateNew(other.Size);

        private EdgeData(GridSize size, double[,] data)
        {
            if (data.GetLength(0) != size.Height || data.GetLength(1) != size.Width)
                throw new ArgumentException("Dimensions don't match.");

            Size = size;
            m_Data = data;
            Bounds = new GridRegion(GridIndex.Zero, Size);
            Inaccessable = Bounds.Shrink(1);
        }

        public bool CanIndex(GridIndex index) => Bounds.IsInside(index) && !Inaccessable.IsInside(index);

        public double this[GridIndex i]
        {
            get
            {
                if(CanIndex(i))
                {
                    return m_Data[i.I, i.J];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if(CanIndex(i))
                {
                    m_Data[i.I, i.J] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public double this[int i, int j]
        {
            get { return this[new GridIndex(i, j)]; }
            set { this[new GridIndex(i, j)] = value; }
        }

        public GridData AsGridData() => GridData.FromArray(m_Data);
    }
}
