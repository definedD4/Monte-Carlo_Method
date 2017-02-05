using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Monte_Carlo_Method_3D.ViewModels;
using ReactiveUI;

namespace Monte_Carlo_Method_3D.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window, IViewFor<SettingsViewModel>
    {
        public SettingsDialog()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(x =>
                {
                    DataContext = x;
                    x?.CloseDialog.Subscribe(_ =>
                    {
                        Close();
                    });
                });

            this.BindCommand(ViewModel, x => x.Ok, x => x.OkBtn);

            this.BindCommand(ViewModel, x => x.Apply, x => x.ApplyBtn);

            this.BindCommand(ViewModel, x => x.Cancel, x => x.CancelBtn);           
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SettingsViewModel)value; }
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
