using System;
using System.Windows;
using System.Windows.Input;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Microsoft.Win32;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StTabViewModel : TabViewModel
    {
        private StSimulator m_Simulator;
        private StVisualizer m_Visualizer;

        private readonly Player m_Player;

        public Pallete Pallete { get; }

        // Property Backing Fields
        private StVisualContext m_VisualContext;

        //Commands
        private readonly DelegateCommand m_StepCommand;
        private readonly SwitchStateCommand m_PlayPauseCommand;
        private readonly DelegateCommand m_RestartCommand;
        private readonly DelegateCommand m_SimulationOptionsCommand;
        private readonly DelegateCommand m_ExportToCsvCommand;

        public StTabViewModel(SimulationOptions options) : base("МСВ")
        {
            Pallete = new Pallete();

            InitComponents(options);          

            // Init player
            m_Player = new Player(() =>
            {
                m_Simulator.SimulateSteps(1);
                VisualContext.UpdateVisualization();
            }, () =>
            {
                m_Simulator.SimulateSteps(20);
                VisualContext.UpdateVisualization();
            }, () =>
            {
                OnPropertyChanged(nameof(SimulationInfo));
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            m_StepCommand = new DelegateCommand(_ =>
            {
                m_Player.StepOnce();
            }, _ => !m_Player.RunningOrPlaying);

            m_PlayPauseCommand = new SwitchStateCommand("Пауза", "Програти", false, _ => !m_Player.SingleStepRunning);
            m_PlayPauseCommand.StateChanged += (s, e) =>
            {
                if (m_PlayPauseCommand.State)
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

            m_RestartCommand = new DelegateCommand(x =>
            {
                m_Simulator.Reset();
                VisualContext.UpdateVisualization();
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !m_Player.RunningOrPlaying);

            m_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_Simulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    InitComponents(result);
                }
            }, x => !m_Player.RunningOrPlaying);

            VisualTypeSelector = new SelectorCommand("2D");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                if (VisualTypeSelector.SelectedValue == "2D")
                {
                    VisualContext = new StVisualContext2D(m_Simulator, m_Visualizer);
                }
                else if (VisualTypeSelector.SelectedValue == "Table")
                {
                    VisualContext = new StTableVisualContext(m_Simulator, m_Visualizer);
                }
                VisualContext.UpdateVisualization();
            };
            VisualTypeSelector.RaiseSelectionChanged();
            VisualTypeSelector.UpdateSelectors();

            m_ExportToCsvCommand = new DelegateCommand(_ =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                     CsvUtil.ExportToFile(m_Simulator.GetData().AsGridData(), saveFileDialog.FileName);
                }
            });
        }

        private void InitComponents(SimulationOptions options)
        {
            m_Simulator = new StSimulator(options);
            m_Visualizer = new StVisualizer(m_Simulator.Size, m_Simulator.StartLocation, Pallete);
            VisualTypeSelector?.RaiseSelectionChanged();
            OnPropertyChanged(nameof(SimulationInfo));
        }

        private void UpdateCommands()
        {
            m_StepCommand.RaiseCanExecuteChanged();
            m_PlayPauseCommand.RaiseCanExecuteChanged();
            m_RestartCommand.RaiseCanExecuteChanged();
        }
        
        public StVisualContext VisualContext
        {
            get { return m_VisualContext; }
            set { m_VisualContext = value; OnPropertyChanged(nameof(VisualContext)); }
        }

        public StSimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        public ICommand StepCommand => m_StepCommand;

        public ICommand PlayPauseCommand => m_PlayPauseCommand;

        public ICommand RestartCommand => m_RestartCommand;

        public SelectorCommand VisualTypeSelector { get; private set; }

        public ICommand SimulationOptionsCommand => m_SimulationOptionsCommand;

        public ICommand ExportToCsvCommand => m_ExportToCsvCommand;
    }
}
