using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Util
{
    public class EdgeData
    {
        private double[] m_DataT; // Top bound
        private double[] m_DataB; // Bottom bound
        private double[] m_DataL; // Left bound
        private double[] m_DataR; // Right bound
        private double[] m_DataC; // Corners: top-right, top-left, bottm-left, bottom-right

        public int Width { get; }
        public int Height { get; }

        public EdgeData(int width, int height)
        {
            Width = width;
            Height = height;

            m_DataT = new double[Width - 2];
            m_DataB = new double[Width - 2];
            m_DataL = new double[Height - 2];
            m_DataR = new double[Height - 2];
            m_DataC = new double[4];
        }

        public double this[int x, int y]
        {
            get
            {
                if (x == 0) // left edge
                {
                    if (y == 0) // top-left
                    {
                        return m_DataC[1];
                    }
                    if (y == Height - 1) // bottom-left
                    {
                        return m_DataC[2];
                    }
                    return m_DataL[y - 1];
                }
                if (x == Width - 1) // right edge
                {
                    if (y == 0) // top-right
                    {
                        return m_DataC[0];
                    }
                    if (y == Height - 1) // bottom-right
                    {
                        return m_DataC[3];
                    }
                    return m_DataR[y - 1];
                }
                if (y == 0) // top edge
                {
                    return m_DataT[x - 1];
                }
                if (y == Height - 1)
                {
                    return m_DataB[x - 1];
                }
                throw new IndexOutOfRangeException("Point coordinates are out of range.");
            }
            set
            {
                if (x == 0) // left edge
                {
                    if (y == 0) // top-left
                    {
                        m_DataC[1] = value;
                    }
                    else if (y == Height - 1) // bottom-left
                    {
                        m_DataC[2] = value;
                    }
                    else
                    {
                        m_DataL[y - 1] = value;
                    }
                }
                else if (x == Width - 1) // right edge
                {
                    if (y == 0) // top-right
                    {
                        m_DataC[0] = value;
                    }
                    else if (y == Height - 1) // bottom-right
                    {
                        m_DataC[3] = value;
                    }
                    else
                    {
                        m_DataR[y - 1] = value;
                    }
                }
                else if (y == 0) // top edge
                {
                    m_DataT[x - 1] = value;
                }
                else if (y == Height - 1)
                {
                    m_DataB[x - 1] = value;
                }
                else throw new IndexOutOfRangeException("Point coordinates are out of range.");
            }
        }
    }
}
