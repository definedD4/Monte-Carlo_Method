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

        [Reactive] public IVisualization Visualization { get; private set; }

        public Pallete Pallete => Settings.Current.VisualizationOptions.Pallete;

        private ReactiveCommand<Unit, Unit> UpdateVisualization { get; }

        public SwitchStateCommand PlayPauseCommand { get; }
        public ReactiveCommand<Unit, Unit> Step { get; }
        public DelegateCommand RestartCommand { get; }
        public DelegateCommand SimulationOptionsCommand { get; }
        public DelegateCommand ExportToCsvCommand { get; }

        public SelectorCommand VisualTypeSelector { get; }

        public PrTabViewModel(SimulationOptions simulationOptions) : base("ІПРАЙ")
        {
            m_SimulationOptions = simulationOptions;

            m_Simulator = new PrSimulator(m_SimulationOptions);

            m_VisualizationProvider = GridDataVisualizationProvider.Table();

            // Init player
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
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator.GetData());
                OnPropertyChanged(nameof(SimulationInfo));
            });

            Step = ReactiveCommand.Create(() =>
            {
                m_Player.StepOnce();
            }, m_Player.WhenAny(x => x.RunningOrPlaying, r => !r.Value));

            PlayPauseCommand = new SwitchStateCommand("Пауза", "Програти", false, _ => !m_Player.SingleStepRunning);
            PlayPauseCommand.StateChanged += (s, e) =>
            {
                if (PlayPauseCommand.State)
                {
                    m_Player.Start();
                }
                else
                {
                    m_Player.Stop();
                }
                UpdateCommands();
            };

            RestartCommand = new DelegateCommand(x =>
            {
                m_Simulator.Reset();
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator.GetData());
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !m_Player.RunningOrPlaying);

            SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(m_SimulationOptions);
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    m_SimulationOptions = dialog.SimulationOptions;
                    m_Simulator = new PrSimulator(m_SimulationOptions);
                    m_VisualizationProvider = GridDataVisualizationProvider.Table();
                    Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator.GetData());
                    OnPropertyChanged(nameof(SimulationInfo));
                }
            }, x => !m_Player.RunningOrPlaying);

            ExportToCsvCommand = new DelegateCommand(x =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    CsvUtil.ExportToFile(m_Simulator.GetData(), saveFileDialog.FileName);
                }
            });

            VisualTypeSelector = new SelectorCommand("Table");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                switch (VisualTypeSelector.SelectedValue)
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
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator.GetData());
            };
            VisualTypeSelector.RaiseSelectionChanged();
            VisualTypeSelector.UpdateSelectors();

            Settings.SettingsCange.InvokeCommand(UpdateVisualization);
        }
        
        private void UpdateCommands()
        {
            PlayPauseCommand.RaiseCanExecuteChanged();
            RestartCommand.RaiseCanExecuteChanged();
            SimulationOptionsCommand.RaiseCanExecuteChanged();
            ExportToCsvCommand.RaiseCanExecuteChanged();
        }

        public PrSimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        
    }
}
