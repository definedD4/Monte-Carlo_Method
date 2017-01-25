using System;
using System.Windows;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Microsoft.Win32;
using Monte_Carlo_Method_3D.SVContext;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StTabViewModel : TabViewModel
    {
        private readonly Player m_Player;

        public Pallete Pallete { get; }

        private SimulationOptions m_SimulationOptions;
        private StSvContext m_SvContext;
        private IVisualization m_Visualization;

        public StTabViewModel(SimulationOptions options) : base("МСВ")
        {
            m_SimulationOptions = options;

            Pallete = new Pallete();       

            m_SvContext = StSvContext.Table(m_SimulationOptions, Pallete);

            // Init player
            m_Player = new Player(() =>
            {
                m_SvContext.Simulator.SimulateSteps(1);
            }, () =>
            {
                m_SvContext.Simulator.SimulateSteps(20);
            }, () =>
            {
                Visualization = m_SvContext.ProvideVisualization();
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
                m_SvContext.Simulator.Reset();
                Visualization = m_SvContext.ProvideVisualization();
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !m_Player.RunningOrPlaying);

            SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(m_SimulationOptions);
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    m_SimulationOptions = dialog.SimulationOptions;
                    m_SvContext = m_SvContext.Clone(m_SimulationOptions);
                    Visualization = m_SvContext.ProvideVisualization();
                    OnPropertyChanged(nameof(SimulationInfo));
                }
            }, x => !m_Player.RunningOrPlaying);

            VisualTypeSelector = new SelectorCommand("2D");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                switch (VisualTypeSelector.SelectedValue)
                {
                    case "Table":
                        m_SvContext = StSvContext.Table(m_SvContext.Options, m_SvContext.Pallete);
                        break;
                    case "2D":
                        m_SvContext = StSvContext.Color(m_SvContext.Options, m_SvContext.Pallete);
                        break;
                }
                Visualization = m_SvContext.ProvideVisualization();
            };
            VisualTypeSelector.RaiseSelectionChanged();
            VisualTypeSelector.UpdateSelectors();

            ExportToCsvCommand = new DelegateCommand(_ =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                     CsvUtil.ExportToFile(m_SvContext.Simulator.GetData().AsGridData(), saveFileDialog.FileName);
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

        public StSimulationInfo SimulationInfo => m_SvContext.Simulator.SimulationInfo;

        public DelegateCommand StepCommand { get; }
        public SwitchStateCommand PlayPauseCommand { get; }
        public DelegateCommand RestartCommand { get; }
        public SelectorCommand VisualTypeSelector { get; }
        public DelegateCommand SimulationOptionsCommand { get; }
        public DelegateCommand ExportToCsvCommand { get; }
    }
}
