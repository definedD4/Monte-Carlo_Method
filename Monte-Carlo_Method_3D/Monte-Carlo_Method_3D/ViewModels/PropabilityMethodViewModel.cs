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
    public class PropabilityMethodViewModel : TabViewModel
    {
        private PropabilityMethodSimulator m_Simulator;
        private PropabilityMethodVisualizer m_Visualizer;
        private IPallete m_Pallete;
        private DispatcherTimer m_Timer;

        private bool p_SimulationInProgress = false;
        public bool SimulationInProgress
        {
            get { return p_SimulationInProgress; }
            set { p_SimulationInProgress = value; OnPropertyChanged(nameof(SimulationInProgress)); UpdateCommands(); }
        }

        public PropabilityMethodViewModel(IPallete pallete) : base("Метод вероятностей")
        {
            m_Pallete = pallete;

            m_Pallete.DrawBlackIfZero = false;
            Gauge = new GaugeContext(pallete);
            m_Pallete.DrawBlackIfZero = true;

            //Init m_Simulator
            m_Simulator = new PropabilityMethodSimulator(5, 5, new IntPoint(2, 2));

            //Init m_Visualizer and visual output
            m_Visualizer = new PropabilityMethodVisualizer(m_Simulator.Width, m_Simulator.Height, pallete) { DrawBorder = false, HeightCoefficient = 50 };
            m_Visualizer.UpdateModelAndTexture(m_Simulator);

            //init visual context
            VisualContext = new PrVisualContext2D(m_Simulator, m_Visualizer);

            //Init m_Timer
            m_Timer = new DispatcherTimer(DispatcherPriority.ContextIdle) {Interval = TimeSpan.FromMilliseconds(10)};
            m_Timer.Tick += (s, e) =>
            {
                if (!SimulationInProgress)
                {
                    SimulationInProgress = true;

                    m_Simulator.SimulateStep();
                    VisualContext.Update();

                    SimulationInProgress = false;

                    OnPropertyChanged(nameof(SimulationInfo));
                }
            };

            c_StepCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateStep();
                VisualContext.Update();

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
                VisualContext.Update();
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            c_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(m_Simulator.Width, m_Simulator.Height, m_Simulator.StartLocation + new IntPoint(1, 1));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    int width = dialog.WidthSetting;
                    int height = dialog.HeightSetting;
                    IntPoint startLocation = new IntPoint(dialog.StartXSetting, dialog.StartYSetting);

                    m_Simulator = new PropabilityMethodSimulator(width, height, startLocation);

                    m_Visualizer = new PropabilityMethodVisualizer(m_Simulator.Width, m_Simulator.Height, pallete) { DrawBorder = false, HeightCoefficient = 25 };
                    VisualContext.Simulator = m_Simulator;
                    VisualContext.Visualizer = m_Visualizer;
                    VisualContext.Update();
                    OnPropertyChanged(nameof(SimulationInfo));
                }
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            c_SimulateToCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;

                while (m_Simulator.Step < SimulateToStep)
                {
                    m_Simulator.SimulateStep();
                }

                VisualContext.Update();

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
                VisualContext.Update();
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

        public SimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        private SwitchStateCommand c_PlayPauseCommand;
        public ICommand PlayPauseCommand => c_PlayPauseCommand;

        private DelegateCommand c_StepCommand;
        public ICommand StepCommand => c_StepCommand;

        private DelegateCommand c_RestartCommand;
        public ICommand RestartCommand => c_RestartCommand;

        private DelegateCommand c_SimulationOptionsCommand;
        public ICommand SimulationOptionsCommand => c_SimulationOptionsCommand;

        private DelegateCommand c_SimulateToCommand;
        public ICommand SimulateToCommand => c_SimulateToCommand;

        private DelegateCommand c_ExportToCsvCommand;
        public ICommand ExportToCsvCommand => c_ExportToCsvCommand;

        private int p_SimulateToStep;
        public int SimulateToStep
        {
            get { return p_SimulateToStep; }
            set
            {
                if (p_SimulateToStep != value)
                {
                    p_SimulateToStep = value; OnPropertyChanged("SimuateToStep");
                    c_SimulateToCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public SelectorCommand VisualTypeSelector { get; private set; }

        private PrVisualContext p_VisualContext;
        public PrVisualContext VisualContext
        {
            get { return p_VisualContext; }
            set
            {
                if (p_VisualContext != value)
                {
                    p_VisualContext = value; OnPropertyChanged("VisualContext");
                }
            }
        }

        public GaugeContext Gauge { get; private set; }
    }
}
