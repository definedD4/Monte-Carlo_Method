using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Views;
using System.Windows;

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

            PrepareJit();

            MainView view = new MainView();
            MainViewModel viewModel = new MainViewModel();
            view.DataContext = viewModel;
            view.Show();
        }

        private void PrepareJit()
        {
            PreJitTypeMethods(typeof(Monte_Carlo_Method_3D.Simulation.PrSimulator));
            PreJitTypeMethods(typeof(Monte_Carlo_Method_3D.Simulation.StSimulator));
        }

        private static void PreJitTypeMethods(Type type)
        {
            MethodInfo[] methods = type.GetMethods(
                BindingFlags.DeclaredOnly |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.Static);

            foreach (MethodInfo curMethod in methods)
            {
                if (curMethod.IsAbstract ||
                    curMethod.ContainsGenericParameters)
                    continue;

                RuntimeHelpers.PrepareMethod(curMethod.MethodHandle);
            }

        }
    }
}
