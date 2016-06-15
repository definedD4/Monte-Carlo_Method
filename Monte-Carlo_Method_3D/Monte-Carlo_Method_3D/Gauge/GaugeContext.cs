using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.Gauge
{
    public class GaugeContext
    {
        public GaugeContext(IPallete pallete)
        {
            double value = 0d;
            for (int i = 0; i < 11; i++)
            {
                Colors[i] = pallete.GetColor(value);
                value += 0.1d;
            }
        }

        public Color[] Colors { get; } = new Color[11]; 
    }
}
