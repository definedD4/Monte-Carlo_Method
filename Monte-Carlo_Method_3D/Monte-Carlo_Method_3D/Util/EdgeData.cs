using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Util
{
    public class EdgeData
    {
        private double[,] m_Data;

        public int Width { get; }
        public int Height { get; }

        public EdgeData(int width, int height)
        {
            Width = width;
            Height = height;

            m_Data = new double[Width, Height];
        }

        public bool CanIndex(int x, int y)
        {
            return x == 0 || x == Width - 1 || y == 0 || y == Width - 1;
        }

        public double this[int x, int y]
        {
            get
            {
                if(CanIndex(x, y))
                {
                    return m_Data[x, y];
                }
                return double.NaN;
            }
            set
            {
                if(CanIndex(x, y))
                {
                    m_Data[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
