using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.Gauge
{
    public class GaugeContext : ViewModelBase
    {
        private Pallete m_Pallete;

        public GaugeContext(Pallete pallete)
        {
            Pallete = pallete;
        }

        public Pallete Pallete
        {
            get { return m_Pallete; }
            set { m_Pallete = value; OnPropertyChanged(nameof(Pallete)); }
        }
    }
}
