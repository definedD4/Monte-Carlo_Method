using System;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Views;
using System.Windows;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainView view = new MainView();
            MainViewModel viewModel = new MainViewModel();
            view.DataContext = viewModel;
            view.Show();
        }
    }
}
