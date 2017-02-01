using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Monte_Carlo_Method_3D.SVContext;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class PrTabViewModel : TabViewModel
    {
        private readonly Player m_Player;

        private SimulationOptions m_SimulationOptions;

        private PrSvContext m_SvContext;
        private IVisualization m_Visualization;

        public Pallete Pallete => VisualizationOptions.Current.Pallete;

        public PrTabViewModel(SimulationOptions simulationOptions) : base("ІПРАЙ")
        {
            m_SimulationOptions = simulationOptions;

            m_SvContext = PrSvContext.Table(m_SimulationOptions);

            // Init player
            m_Player = new Player(() =>
            {
                m_SvContext.Simulator.SimulateSteps();
            }, () =>
            {
                m_SvContext.Simulator.SimulateSteps(5);             
            }, () =>
            {
                Visualization = m_SvContext.ProvideVisualization();
                OnPropertyChanged(nameof(SimulationInfo));
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            StepCommand = new DelegateCommand(x =>
            {
                m_Player.StepOnce();
            }, x => !m_Player.RunningOrPlaying);

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

            ExportToCsvCommand = new DelegateCommand(x =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    CsvUtil.ExportToFile(m_SvContext.Simulator.GetData(), saveFileDialog.FileName);
                }
            });

            VisualTypeSelector = new SelectorCommand("Table");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                switch (VisualTypeSelector.SelectedValue)
                {
                    case "Table":
                        m_SvContext = PrSvContext.Table(m_SvContext.SimulationOptions);
                        break;
                    case "2D":
                        m_SvContext = PrSvContext.Color(m_SvContext.SimulationOptions);
                        break;
                    case "3D":
                        m_SvContext = PrSvContext.Model3D(m_SvContext.SimulationOptions);
                        break;
                }
                Visualization = m_SvContext.ProvideVisualization();
            };
            VisualTypeSelector.RaiseSelectionChanged();
            VisualTypeSelector.UpdateSelectors();
        }
        
        private void UpdateCommands()
        {
            StepCommand.RaiseCanExecuteChanged();
            PlayPauseCommand.RaiseCanExecuteChanged();
            RestartCommand.RaiseCanExecuteChanged();
            SimulationOptionsCommand.RaiseCanExecuteChanged();
            ExportToCsvCommand.RaiseCanExecuteChanged();
        }

        public PrSimulationInfo SimulationInfo => m_SvContext.Simulator.SimulationInfo;

        public SwitchStateCommand PlayPauseCommand { get; }
        public DelegateCommand StepCommand { get; }
        public DelegateCommand RestartCommand { get; }
        public DelegateCommand SimulationOptionsCommand { get; }
        public DelegateCommand ExportToCsvCommand { get; }

        public SelectorCommand VisualTypeSelector { get; }

        public IVisualization Visualization
        {
            get { return m_Visualization; }
            set { m_Visualization = value; OnPropertyChanged(nameof(Visualization)); }
        }
    }
}
