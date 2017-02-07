using System;
using System.Reactive;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Microsoft.Win32;
using Monte_Carlo_Method_3D.AppSettings;
using Monte_Carlo_Method_3D.Util.Commands;
using Monte_Carlo_Method_3D.VisualizationModel;
using Monte_Carlo_Method_3D.VisualizationProvider;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StTabViewModel : TabViewModel
    {
        private readonly Player m_Player;

        private SimulationOptions m_SimulationOptions;

        private StSimulator m_Simulator;

        private StVisualizationProvider m_VisualizationProvider;

        [Reactive]
        public IVisualization Visualization { get; private set; }

        [Reactive]
        public Pallete Pallete { get; private set; }

        private ReactiveCommand<Unit, Unit> UpdateVisualization { get; }

        public ReactivePlayPauseCommand PlayPause { get; }

        public ReactiveCommand<Unit, Unit> Step { get; }

        public ReactiveCommand<Unit, Unit> Restart { get; }

        public ReactiveCommand<Unit, Unit> OpenSimulationOptions { get; }

        public ReactiveCommand<Unit, Unit> ExportToCsv { get; }

        public ReactiveSelectorCommand<string> VisualTypeSelector { get; }

        [Reactive] public StSimulationInfo SimulationInfo { get; private set; }

        public StTabViewModel(SimulationOptions simulationOptions) : base("МСВ")
        {
            m_SimulationOptions = simulationOptions;

            m_Simulator = new StSimulator(m_SimulationOptions);

            m_VisualizationProvider = StVisualizationProvider.Table();

            m_Player = new Player(() =>
            {
                m_Simulator.SimulateSteps();
            }, () =>
            {
                m_Simulator.SimulateSteps(5);
            }, () =>
            {
                UpdateVisualization.Execute().Subscribe();
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            UpdateVisualization = ReactiveCommand.Create(() =>
            {
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator);
                SimulationInfo = m_Simulator.SimulationInfo;
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
                            m_VisualizationProvider = StVisualizationProvider.Table();
                            break;
                        case "2D":
                            m_VisualizationProvider = StVisualizationProvider.Color();
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

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    m_SimulationOptions = dialog.SimulationOptions;
                    m_Simulator = new StSimulator(m_SimulationOptions);
                    m_VisualizationProvider = StVisualizationProvider.Table();
                }
            }, notRunningOrPlaying);

            OpenSimulationOptions
                .InvokeCommand(UpdateVisualization);

            ExportToCsv = ReactiveCommand.Create(() =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    CsvUtil.ExportToFile(m_Simulator.GetData().AsGridData(), saveFileDialog.FileName);
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
