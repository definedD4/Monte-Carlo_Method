using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.Visualization
{
    public class HSVPallete : IPallete
    {
        public HSVPallete()
        {

        }

        public bool DrawBlackIfZero { get; set; } = true;

        public Color GetColor(double value)
        {
            if (DrawBlackIfZero && value == 0)
            {
                return Colors.Black;
            }
            double val = Math.Sqrt(Math.Sqrt(Math.Sqrt(Math.Sqrt(value)))) * 0.625 + 0.325;
            double h = ((1d - val) * 360);
            int i = (int)(h / 60);
            double a = (h % 60) * 5 / 3;
            byte ac = (byte)(int)(a * 255 / 100);
            byte _ac = (byte)(int)((100 - a) * 255 / 100);
            switch (i)
            {
                case 0:
                    return Color.FromRgb(255, ac, 0);
                case 1:
                    return Color.FromRgb(_ac, 255, 0);
                case 2:
                    return Color.FromRgb(0, 255, ac);
                case 3:
                    return Color.FromRgb(0, _ac, 255);
                case 4:
                    return Color.FromRgb(ac, 0, 255);
                case 5:
                    return Color.FromRgb(255, 0, _ac);
                default:
                    return Colors.Black;
            }
        }
    }
}
