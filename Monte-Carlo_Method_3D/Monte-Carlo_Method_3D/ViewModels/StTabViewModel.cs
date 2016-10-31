using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Microsoft.Win32;
using Monte_Carlo_Method_3D.Gauge;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StTabViewModel : TabViewModel
    {
        private StSimulator m_Simulator;
        private StVisualizer m_Visualizer;

        private DispatcherTimer m_Timer;
        private Pallete m_Pallete;

        // Property Backing Fields
        private bool m_SimulationInProgress = false;
        private StVisualContext m_VisualContext;
        private GaugeContext m_Gauge;

        //Commands
        private readonly DelegateCommand m_StepCommand;
        private readonly SwitchStateCommand m_PlayPauseCommand;
        private readonly DelegateCommand m_RestartCommand;
        private readonly DelegateCommand m_SimulationOptionsCommand;
        private readonly DelegateCommand m_ExportToCsvCommand;

        public StTabViewModel(SimulationOptions options) : base("Метод статистических испытаний")
        {
            m_Pallete = new Pallete();

            InitComponents(options);

            Gauge = new GaugeContext(m_Pallete);

            m_StepCommand = new DelegateCommand(_ =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateSteps(1);
                m_VisualContext.UpdateVisualization();

                SimulationInProgress = false;

                OnPropertyChanged(nameof(SimulationInfo));
            }, _ => !(SimulationInProgress || m_Timer.IsEnabled));

            m_PlayPauseCommand = new SwitchStateCommand("Пауза", "Воспроизвести", false, _ => !(SimulationInProgress && !m_Timer.IsEnabled));
            m_PlayPauseCommand.StateChanged += (s, e) =>
            {
                if (m_PlayPauseCommand.State)
                {
                    m_Timer.Start();
                    UpdateCommands();
                }
                else
                {
                    m_Timer.Stop();
                    UpdateCommands();
                }
            };

            m_Timer = new DispatcherTimer(DispatcherPriority.ContextIdle) { Interval = TimeSpan.FromMilliseconds(10) };
            m_Timer.Tick += (s, e) =>
            {
                if (!SimulationInProgress)
                {
                    SimulationInProgress = true;

                    m_Simulator.SimulateSteps(200);
                    VisualContext.UpdateVisualization();

                    SimulationInProgress = false;

                    OnPropertyChanged(nameof(SimulationInfo));
                }
            };

            m_RestartCommand = new DelegateCommand(x =>
            {
                m_Simulator.Reset();
                VisualContext.UpdateVisualization();
                OnPropertyChanged(nameof(SimulationInfo));
            }, _ => !(SimulationInProgress || m_Timer.IsEnabled));

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

            m_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_Simulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    InitComponents(result);
                }
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            m_ExportToCsvCommand = new DelegateCommand(_ =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    double[,] data = new double[m_Simulator.Width, m_Simulator.Height];

                    for(int x = 0; x < m_Simulator.Width; x++)
                    {
                        data[x, 0] = m_Simulator[x, 0];
                        data[x, m_Simulator.Height - 1] = m_Simulator[x, m_Simulator.Height - 1];
                    }

                    for (int y = 1; y < m_Simulator.Height - 1; y++)
                    {
                        data[0, y] = m_Simulator[0, y];
                        data[m_Simulator.Width - 1, y] = m_Simulator[m_Simulator.Width - 1, y];
                    }

                    new CsvExporter(';').ExportToFile(data, saveFileDialog.FileName);
                }
            });


        }

        private void InitComponents(SimulationOptions options)
        {
            m_Simulator = new StSimulator(options);
            m_Visualizer = new StVisualizer(m_Simulator, m_Pallete);
            VisualTypeSelector?.RaiseSelectionChanged();
            OnPropertyChanged(nameof(SimulationInfo));
        }

        private void UpdateCommands()
        {
            m_StepCommand.RaiseCanExecuteChanged();
            m_PlayPauseCommand.RaiseCanExecuteChanged();
            m_RestartCommand.RaiseCanExecuteChanged();
        }

        public bool SimulationInProgress
        {
            get { return m_SimulationInProgress; }
            private set { m_SimulationInProgress = value; OnPropertyChanged(nameof(m_SimulationInProgress)); m_StepCommand.RaiseCanExecuteChanged(); }
        }

        public StVisualContext VisualContext
        {
            get { return m_VisualContext; }
            set { m_VisualContext = value; OnPropertyChanged(nameof(VisualContext)); }
        }

        public GaugeContext Gauge
        {
            get { return m_Gauge; }
            set { m_Gauge = value; OnPropertyChanged(nameof(Gauge)); }
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
