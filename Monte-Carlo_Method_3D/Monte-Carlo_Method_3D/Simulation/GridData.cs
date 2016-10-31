using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class GridData
    {
        private readonly double[,] m_Data;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public GridData(double[,] data) : this(data.GetLength(0), data.GetLength(1), data) { }

        public GridData(int width, int height, double[,] data)
        {
            if(data.GetLength(0) != width || data.GetLength(1) != height)
                throw new ArgumentException("Dimensions don't match.");

            Width = width;
            Height = height;
            m_Data = data;
        }

        public double this[int x, int y] => m_Data[x, y];
    }
}
