using System;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Microsoft.Win32;
using Monte_Carlo_Method_3D.VisualizationModel;
using Monte_Carlo_Method_3D.VisualizationProvider;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StTabViewModel : TabViewModel
    {
        private readonly Player m_Player;

        private SimulationOptions m_SimulationOptions;

        private StSimulator m_Simulator;

        private StVisualizationProvider m_VisualizationProvider;
        private IVisualization m_Visualization;

        public Pallete Pallete => VisualizationOptions.Current.Pallete;

        public StTabViewModel(SimulationOptions options) : base("МСВ")
        {
            m_SimulationOptions = options;

            m_Simulator = new StSimulator(m_SimulationOptions);

            m_VisualizationProvider = StVisualizationProvider.Table();

            // Init player
            m_Player = new Player(() =>
            {
                m_Simulator.SimulateSteps(1);
            }, () =>
            {
                m_Simulator.SimulateSteps(20);
            }, () =>
            {
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator);
                OnPropertyChanged(nameof(SimulationInfo));
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            StepCommand = new DelegateCommand(_ =>
            {
                m_Player.StepOnce();
            }, _ => !m_Player.RunningOrPlaying);

            PlayPauseCommand = new SwitchStateCommand("Пауза", "Програти", false, _ => !m_Player.SingleStepRunning);
            PlayPauseCommand.StateChanged += (s, e) =>
            {
                if (PlayPauseCommand.State)
                {
                    m_Player.Start();
                    UpdateCommands();
                }
                else
                {
                    m_Player.Stop();
                    UpdateCommands();
                }
            };

            RestartCommand = new DelegateCommand(x =>
            {
                m_Simulator.Reset();
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator);
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !m_Player.RunningOrPlaying);

            SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(m_SimulationOptions);
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    m_SimulationOptions = dialog.SimulationOptions;
                    m_Simulator = new StSimulator(m_SimulationOptions);
                    m_VisualizationProvider = StVisualizationProvider.Table();
                    Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator);
                    OnPropertyChanged(nameof(SimulationInfo));
                }
            }, x => !m_Player.RunningOrPlaying);

            VisualTypeSelector = new SelectorCommand("2D");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                switch (VisualTypeSelector.SelectedValue)
                {
                    case "Table":
                        m_VisualizationProvider = StVisualizationProvider.Table();
                        break;
                    case "2D":
                        m_VisualizationProvider = StVisualizationProvider.Color();
                        break;
                }
                Visualization = m_VisualizationProvider.ProvideVisualization(m_Simulator);
            };
            VisualTypeSelector.RaiseSelectionChanged();
            VisualTypeSelector.UpdateSelectors();

            ExportToCsvCommand = new DelegateCommand(_ =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                     CsvUtil.ExportToFile(m_Simulator.GetData().AsGridData(), saveFileDialog.FileName);
                }
            });
        }

        private void UpdateCommands()
        {
            StepCommand.RaiseCanExecuteChanged();
            PlayPauseCommand.RaiseCanExecuteChanged();
            RestartCommand.RaiseCanExecuteChanged();
        }
        
        public IVisualization Visualization
        {
            get { return m_Visualization; }
            set { m_Visualization = value; OnPropertyChanged(nameof(Visualization)); }
        }

        public StSimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        public DelegateCommand StepCommand { get; }
        public SwitchStateCommand PlayPauseCommand { get; }
        public DelegateCommand RestartCommand { get; }
        public SelectorCommand VisualTypeSelector { get; }
        public DelegateCommand SimulationOptionsCommand { get; }
        public DelegateCommand ExportToCsvCommand { get; }
    }
}
