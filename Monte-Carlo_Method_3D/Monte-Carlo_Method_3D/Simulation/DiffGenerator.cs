using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class DiffGenerator
    {
        private PrSimulator m_PrSimulator;
        private StSimulator m_StSimulator;

        public DiffGenerator(PrSimulator prSimulator, StSimulator stSimulator)
        {
            if(prSimulator.Width != stSimulator.Width || prSimulator.Height != stSimulator.Height)
                throw new ArgumentException("Simulators must be equal size.");

            m_PrSimulator = prSimulator;
            m_StSimulator = stSimulator;
        }

        public int Width => m_PrSimulator.Width;
        public int Height => m_PrSimulator.Height;

        public double this[int x, int y] => m_PrSimulator[x, y] - m_StSimulator[x, y];

        public bool CanIndex(int x, int y)
        {
            return x == 0 || x == Width - 1 || y == 0 || y == Width - 1;
        }
    }
}
