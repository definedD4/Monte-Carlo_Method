using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Gauge;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Pallete pallete;

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>();

            pallete = new Pallete();

            PrTabViewModel propabilityMethodViewModel = new PrTabViewModel(pallete);

            propabilityMethodViewModel.PropertyChanged += (s, e) =>
            {
                RaisePropertyChanged(e);
            };

            Tabs.Add(propabilityMethodViewModel);
            SelectedTab = propabilityMethodViewModel;
            Tabs.Add(new StTabViewModel());
            Tabs.Add(new CpTabViewModel());
            //Tabs.Add(new СlTabViewModel());
        }

        public ObservableCollection<TabViewModel> Tabs { get; private set; }
        private TabViewModel p_SelectedTab;
        public TabViewModel SelectedTab
        {
            get { return p_SelectedTab; }
            set
            {
                if(p_SelectedTab != value)
                {
                    p_SelectedTab = value;
                    OnPropertyChanged("SelectedTab");
                }
            }
        }

    }
}
