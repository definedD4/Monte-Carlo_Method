﻿using System;

namespace Monte_Carlo_Method_3D.DataModel
{
    public class GridData
    {
        private readonly double[,] m_Data;

        public GridSize Size { get; }
        public GridRegion Bounds { get; }

        public static GridData FromArray(double[,] data) => new GridData(new GridSize(data.GetLength(1), data.GetLength(0)), data);
        public static GridData AllocateNew(GridSize size) => new GridData(size, new double[size.Height, size.Width]);
        public static GridData AllocateLike(GridData other) => GridData.AllocateNew(other.Size);

        private GridData(GridSize size, double[,] data)
        {
            if(data.GetLength(0) != size.Height || data.GetLength(1) != size.Width)
                throw new ArgumentException("Dimensions don't match.");

            Size = size;
            m_Data = data;
            Bounds = new GridRegion(GridIndex.Zero, Size);
        }

        public bool CanIndex(GridIndex index) => Bounds.IsInside(index);

        public double this[GridIndex i]
        {
            get
            {
                if (CanIndex(i))
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
                if (CanIndex(i))
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

        public double[,] Get()
        {
            return m_Data;
        }

        public EdgeData AsEdgeData() => EdgeData.FromArray(m_Data);
    }
}
