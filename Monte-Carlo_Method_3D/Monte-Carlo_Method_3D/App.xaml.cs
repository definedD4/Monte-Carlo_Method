using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Monte_Carlo_Method_3D.ViewModels;
using Monte_Carlo_Method_3D.Views;
using System.Windows;
using Monte_Carlo_Method_3D.Util;

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

            Logger.Threshold = LogLevel.Debug;

            var logger = Logger.New(typeof(App));
            
            Logger.Messages
                .Subscribe(message =>
                {
                    Console.WriteLine($"[{message.LogLevel}] [{message.LoggerName}] (Thread: {message.ThreadId}) {message.Message}");
                });

            using (logger.LogPerf("Prepare JIT"))
            {
                PrepareJit();
            }

            MainView view = new MainView();
            MainViewModel viewModel = new MainViewModel();
            view.DataContext = viewModel;
            view.Show();

            logger.LogInfo($"Logger threshold is {Logger.Threshold}");
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
