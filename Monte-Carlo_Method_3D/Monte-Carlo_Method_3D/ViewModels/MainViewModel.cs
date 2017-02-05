using Monte_Carlo_Method_3D.Simulation;
using System.Collections.ObjectModel;
using System.Linq;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private TabViewModel m_SelectedTab;

        public DelegateCommand SettingsCommand { get; }

        public MainViewModel()
        {
            SettingsCommand = new DelegateCommand(_ =>
            {
                var dlg = new SettingsDialog {ViewModel = new SettingsViewModel()};
                dlg.ShowDialog();
            });

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
