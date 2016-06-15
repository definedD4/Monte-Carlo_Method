using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.Visualization
{
    public interface IPallete
    {
        Color GetColor(double value);

        bool DrawBlackIfZero { get; set; }
    }
}
