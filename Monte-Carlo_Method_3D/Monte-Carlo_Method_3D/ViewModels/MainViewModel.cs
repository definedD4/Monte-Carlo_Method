using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Gauge;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private TabViewModel m_SelectedTab;

        public MainViewModel()
        {
            SimulationOptions options = new SimulationOptions(new GridSize(9, 9), new GridIndex(4, 4));

            Tabs.Add(new PrTabViewModel(options));
            Tabs.Add(new StTabViewModel(options));
            Tabs.Add(new CpTabViewModel(options));
            Tabs.Add(new СlTabViewModel());

            SelectedTab = Tabs.First();
        }

        public ObservableCollection<TabViewModel> Tabs { get; } = new ObservableCollection<TabViewModel>();

        public TabViewModel SelectedTab
        {
            get { return m_SelectedTab; }
            set { m_SelectedTab = value; OnPropertyChanged(nameof(SelectedTab)); }
        }

    }
}
