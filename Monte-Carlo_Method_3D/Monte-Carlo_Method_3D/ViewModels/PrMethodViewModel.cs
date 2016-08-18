using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Gauge;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class PrMethodViewModel : TabViewModel
    {
        private PrSimulator m_Simulator;
        private PrVisualizer m_Visualizer;
        private Pallete m_Pallete;
        private DispatcherTimer m_Timer;
        private PrVisualContext m_VisualContext;
        private GaugeContext m_Gauge;

        private bool m_SimulationInProgress = false;
        private long m_SimulateToStep;

        private SwitchStateCommand c_PlayPauseCommand;
        private DelegateCommand c_StepCommand;
        private DelegateCommand c_RestartCommand;
        private DelegateCommand c_SimulationOptionsCommand;
        private DelegateCommand c_SimulateToCommand;

        public PrMethodViewModel(Pallete pallete) : base("Метод вероятностей")
        {
            m_Pallete = pallete;

            Gauge = new GaugeContext(pallete);

            //Init m_Simulator
            m_Simulator = new PrSimulator(5, 5, new IntPoint(2, 2));

            //Init m_Visualizer and visual output
            m_Visualizer = new PrVisualizer(m_Simulator, pallete);

            //init visual context
            VisualContext = new PrVisualContext2D(m_Simulator, m_Visualizer);
            VisualContext.UpdateVisualization();

            //Init timer
            m_Timer = new DispatcherTimer(DispatcherPriority.ContextIdle) {Interval = TimeSpan.FromMilliseconds(10)};
            m_Timer.Tick += (s, e) =>
            {
                if (!SimulationInProgress)
                {
                    SimulationInProgress = true;

                    m_Simulator.SimulateSteps(5);
                    VisualContext.UpdateVisualization();

                    SimulationInProgress = false;

                    OnPropertyChanged(nameof(SimulationInfo));
                }
            };

            c_StepCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateSteps();
                VisualContext.UpdateVisualization();

                SimulationInProgress = false;
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            c_PlayPauseCommand = new SwitchStateCommand("Пауза", "Воспроизвести", false, _ => !(SimulationInProgress && !m_Timer.IsEnabled));
            c_PlayPauseCommand.StateChanged += (s, e) =>
            {
                if (c_PlayPauseCommand.State)
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

            c_RestartCommand = new DelegateCommand(x =>
            {
                m_Simulator.Reset();
                VisualContext.UpdateVisualization();
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            c_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_Simulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    m_Simulator = new PrSimulator(result.Width, result.Height, result.StartLocation);
                    m_Visualizer = new PrVisualizer(m_Simulator, pallete);
                    VisualContext.Simulator = m_Simulator;
                    VisualContext.Visualizer = m_Visualizer;
                    VisualContext.UpdateVisualization();
                    OnPropertyChanged(nameof(SimulationInfo));
                }
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            c_SimulateToCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateSteps(Math.Max(SimulateToStep - m_Simulator.Step, 0L));

                VisualContext.UpdateVisualization();

                SimulationInProgress = false;
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled || SimulateToStep <= 0));

            c_ExportToCsvCommand = new DelegateCommand(x =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    new CsvExporter(';').ExportToFile(m_Simulator.GetData(), saveFileDialog.FileName);
                }
            });

            VisualTypeSelector = new SelectorCommand("2D");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                if(VisualTypeSelector.SelectedValue == "2D" && !(VisualContext is PrVisualContext2D))
                {
                    VisualContext = new PrVisualContext2D(m_Simulator, m_Visualizer);
                }
                else if(VisualTypeSelector.SelectedValue == "3D" && !(VisualContext is PrVisualContext3D))
                {
                    VisualContext = new PrVisualContext3D(m_Simulator, m_Visualizer);
                }
                else if (VisualTypeSelector.SelectedValue == "Table" && !(VisualContext is PrTableVisualContext))
                {
                    VisualContext = new PrTableVisualContext(m_Simulator, m_Visualizer);
                }
                VisualContext.UpdateVisualization();
            };

            VisualTypeSelector.UpdateSelectors();
        }

        private void UpdateCommands()
        {
            c_StepCommand.RaiseCanExecuteChanged();
            c_PlayPauseCommand.RaiseCanExecuteChanged();
            c_RestartCommand.RaiseCanExecuteChanged();
            c_SimulationOptionsCommand.RaiseCanExecuteChanged();
            c_SimulateToCommand.RaiseCanExecuteChanged();
            c_ExportToCsvCommand.RaiseCanExecuteChanged();
        }

        public bool SimulationInProgress
        {
            get { return m_SimulationInProgress; }
            set { m_SimulationInProgress = value; OnPropertyChanged(nameof(SimulationInProgress)); UpdateCommands(); }
        }

        public PrSimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        public ICommand PlayPauseCommand => c_PlayPauseCommand;
        public ICommand StepCommand => c_StepCommand;
        public ICommand RestartCommand => c_RestartCommand;
        public ICommand SimulationOptionsCommand => c_SimulationOptionsCommand;
        public ICommand SimulateToCommand => c_SimulateToCommand;
        private DelegateCommand c_ExportToCsvCommand;
        public ICommand ExportToCsvCommand => c_ExportToCsvCommand;

        public long SimulateToStep
        {
            get { return m_SimulateToStep; }
            set { m_SimulateToStep = value; OnPropertyChanged("SimuateToStep"); c_SimulateToCommand.RaiseCanExecuteChanged(); }
        }

        public SelectorCommand VisualTypeSelector { get; private set; }

        public PrVisualContext VisualContext
        {
            get { return m_VisualContext; }
            set
            {
                if (m_VisualContext != value)
                {
                    m_VisualContext = value; OnPropertyChanged("VisualContext");
                }
            }
        }

        public GaugeContext Gauge
        {
            get { return m_Gauge; }
            set { m_Gauge = value;  OnPropertyChanged(nameof(Gauge)); }
        }
    }
}
