using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Monte_Carlo_Method_3D.AppSettings;
using Monte_Carlo_Method_3D.Util.Commands;
using Monte_Carlo_Method_3D.VisualizationModel;
using Monte_Carlo_Method_3D.VisualizationProvider;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class PrTabViewModel : TabViewModel
    {
        private readonly Player m_Player;

        private SimulationOptions m_SimulationOptions;

        private PrSimulator m_Simulator;

        private GridDataVisualizationProvider m_VisualizationProvider;

        [Reactive]
        public IVisualization Visualization { get; private set; }

        [Reactive]
        public Pallete Pallete { get; private set; }

        private ReactiveCommand<Unit, Unit> UpdateVisualization { get; }

        public ReactiveCommand<Unit, Unit> Step { get; }

        public ReactivePlayPauseCommand PlayPause { get; }
        
        public ReactiveCommand<Unit, Unit> Restart { get; }

        public ReactiveCommand<Unit, Unit> OpenSimulationOptions { get; }

        public ReactiveCommand<Unit, Unit> ExportToCsv { get; }

        public ReactiveSelectorCommand<string> VisualTypeSelector { get; }

        [Reactive] public PrSimulationInfo SimulationInfo { get; private set; }

        public PrTabViewModel(SimulationOptions simulationOptions) : base("ІПРАЙ")
        {
            var logger = Logger.New(typeof(PrTabViewModel));

            using (logger.LogPerf("Init"))
            {
                m_SimulationOptions = simulationOptions;

                m_Simulator = new PrSimulator(m_SimulationOptions);

                m_VisualizationProvider = GridDataVisualizationProvider.Table();

                m_Player = new Player(() =>
                {
                    using (logger.LogPerf("Step simulation"))
                    {
                        m_Simulator.SimulateSteps();
                    }
                }, () =>
                {
                    using (logger.LogPerf("Auto simulation"))
                    {
                        m_Simulator.SimulateSteps(5);
                    }
                }, () =>
                {
                    UpdateVisualization.Execute().Subscribe();
                }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

                UpdateVisualization = ReactiveCommand.Create(() =>
                {
                    using (logger.LogPerf("Visualization"))
                    {
                        Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator.GetData());
                        SimulationInfo = m_Simulator.SimulationInfo;
                    }
                });

                var notRunningOrPlaying = m_Player.WhenAny(x => x.RunningOrPlaying, r => !r.Value);

                var notSingleStepRunning = m_Player.WhenAny(x => x.SingleStepRunning, r => !r.Value);

                Step = ReactiveCommand.Create(() =>
                {
                    m_Player.StepOnce();
                }, notRunningOrPlaying);

                PlayPause = new ReactivePlayPauseCommand("Програти", "Пауза", notSingleStepRunning);

                PlayPause.Subscribe(p =>
                {
                    if (p)
                    {
                        m_Player.Start();
                    }
                    else
                    {
                        m_Player.Stop();
                    }
                });

                Restart = ReactiveCommand.Create(() =>
                {
                    m_Simulator.Reset();
                }, notRunningOrPlaying);

                Restart.InvokeCommand(UpdateVisualization);

                VisualTypeSelector = new ReactiveSelectorCommand<string>(x => (string)x, "Table");

                VisualTypeSelector.Selected
                    .Subscribe(option =>
                    {
                        switch (option)
                        {
                            case "Table":
                                m_VisualizationProvider = GridDataVisualizationProvider.Table();
                                break;
                            case "2D":
                                m_VisualizationProvider = GridDataVisualizationProvider.Color();
                                break;
                            case "3D":
                                m_VisualizationProvider = GridDataVisualizationProvider.Model3D(m_SimulationOptions.Size);
                                break;
                        }
                    });

                VisualTypeSelector.Selected
                    .ToSignal()
                    .InvokeCommand(UpdateVisualization);

                OpenSimulationOptions = ReactiveCommand.Create(() =>
                {
                    SimulationOptionsDialog dialog = new SimulationOptionsDialog(m_SimulationOptions);
                    dialog.ShowDialog();

                    if (!dialog.DialogResult.GetValueOrDefault(false)) return;

                    m_SimulationOptions = dialog.SimulationOptions;
                    m_Simulator = new PrSimulator(m_SimulationOptions);
                    m_VisualizationProvider = m_VisualizationProvider.Copy(m_SimulationOptions.Size);
                }, notRunningOrPlaying);

                OpenSimulationOptions
                    .InvokeCommand(UpdateVisualization);

                ExportToCsv = ReactiveCommand.Create(() =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                    if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                    {
                        CsvUtil.ExportToFile(m_Simulator.GetData(), saveFileDialog.FileName);
                    }
                });

                Settings.SettingsChange
                    .InvokeCommand(UpdateVisualization);

                Settings.SettingsChange
                    .Subscribe(_ =>
                    {
                        Pallete = Settings.Current.VisualizationOptions.Pallete;
                    });

                Pallete = Settings.Current.VisualizationOptions.Pallete;
            }
        }
    }
}
