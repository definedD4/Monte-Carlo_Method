using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Simulation
{
    public class DiffGenerator
    {
        private PrSimulator m_PrSimulator;
        private StSimulator m_StSimulator;

        public DiffGenerator(PrSimulator prSimulator, StSimulator stSimulator)
        {
            if(prSimulator.Size != stSimulator.Size)
                throw new ArgumentException("Simulators must be equal size.");

            m_PrSimulator = prSimulator;
            m_StSimulator = stSimulator;

            Size = m_PrSimulator.Size;
        }

        public GridSize Size { get; }

        public EdgeData GetData()
        {
            var prData = m_PrSimulator.GetData();
            var stData = m_StSimulator.GetData();
            var data = EdgeData.AllocateLike(stData);
            foreach(var i in stData.Bounds.EnumerateEdge())
            {
                data[i] = stData[i] - prData[i];
            }
            return data;
        }
    }
}
