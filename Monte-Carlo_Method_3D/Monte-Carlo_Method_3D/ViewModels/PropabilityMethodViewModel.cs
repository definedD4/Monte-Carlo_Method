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
using System.Windows.Input;
using System.Windows.Threading;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class PropabilityMethodViewModel : TabViewModel
    {
        private static readonly string str_play = "Воспроизвести";
        private static readonly string str_pause = "Пауза";

        private PropabilityMethodSimulator simulator;
        private PropabilityMethodVisualizer visualizer;
        private IPallete pallete;
        private DispatcherTimer timer;

        public bool SimulationInProgress { get; private set; }

        public PropabilityMethodViewModel(IPallete pallete) : base("Метод вероятностей")
        {
            this.pallete = pallete;

            pallete.DrawBlackIfZero = false;
            Gauge = new GaugeContext(pallete);
            pallete.DrawBlackIfZero = true;

            //Init simulator
            simulator = new PropabilityMethodSimulator(100, 100, new IntPoint(50, 50));

            //Init visualizer and visual output
            visualizer = new PropabilityMethodVisualizer(simulator.Width, simulator.Height, pallete) { DrawBorder = false, HeightCoefficient = 50 };
            visualizer.UpdateModelAndTexture(simulator);

            //init visual context
            VisualContext = new VisualContext2D(simulator, visualizer);

            //Init timer
            timer = new DispatcherTimer(DispatcherPriority.ContextIdle);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += (s, e) =>
            {
                if (!SimulationInProgress)
                {
                    SimulationInProgress = true;

                    simulator.SimulateStep();
                    VisualContext.Update();

                    SimulationInProgress = false;
                }
            };

            c_StepCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;
                UpdateCommands();

                simulator.SimulateStep();
                VisualContext.Update();

                SimulationInProgress = false;
                UpdateCommands();
            }, x => !(SimulationInProgress || timer.IsEnabled));


            PlayPauseText = str_play;
            c_PlayPauseCommand = new DelegateCommand(x =>
            {
                if (PlayPauseText == str_play)
                {
                    PlayPauseText = str_pause;
                    timer.Start();
                    UpdateCommands();
                }
                else if (PlayPauseText == str_pause)
                {
                    PlayPauseText = str_play;
                    timer.Stop();
                    UpdateCommands();
                }
            }, x => !(SimulationInProgress && !timer.IsEnabled));

            c_RestartCommand = new DelegateCommand(x =>
            {
                simulator.Reset();
                VisualContext.Update();
            }, x => !(SimulationInProgress || timer.IsEnabled));

            c_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(simulator.Width, simulator.Height, simulator.StartLocation);
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    int width = dialog.WidthSetting;
                    int height = dialog.HeightSetting;
                    IntPoint startLocation = new IntPoint(dialog.StartXSetting, dialog.StartYSetting);

                    simulator = new PropabilityMethodSimulator(width, height, startLocation);
                    AddListenedObject(simulator);

                    visualizer = new PropabilityMethodVisualizer(simulator.Width, simulator.Height, pallete) { DrawBorder = false, HeightCoefficient = 25 };
                    VisualContext.Simulator = simulator;
                    VisualContext.Visualizer = visualizer;
                    VisualContext.Update();
                }
            }, x => !(SimulationInProgress || timer.IsEnabled));

            c_SimulateToCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;
                UpdateCommands();

                while (Step < SimulateToStep)
                {
                    simulator.SimulateStep();
                }

                VisualContext.Update();

                SimulationInProgress = false;
                UpdateCommands();
            }, x => !(SimulationInProgress || timer.IsEnabled || SimulateToStep <= 0));

            VisualTypeSelector = new SelectorCommand("2D");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                if(VisualTypeSelector.SelectedValue == "2D" && !(VisualContext is VisualContext2D))
                {
                    VisualContext = new VisualContext2D(simulator, visualizer);
                }
                else if(VisualTypeSelector.SelectedValue == "3D" && !(VisualContext is VisualContext3D))
                {
                    VisualContext = new VisualContext3D(simulator, visualizer);
                }
                else if (VisualTypeSelector.SelectedValue == "Table" && !(VisualContext is TableVisualContext))
                {
                    VisualContext = new TableVisualContext(simulator, visualizer);
                }
                VisualContext.Update();
            };

            VisualTypeSelector.UpdateSelectors();

            AddListenedObject(simulator);

            RegisterPropertyDependency("Step", "Step");
            RegisterPropertyDependency("TotalSimTime", "TotalSimTime");
            RegisterPropertyDependency("EdgeSum", "EdgeSum");
            RegisterPropertyDependency("CenterSum", "CenterSum");
        }

        private void UpdateCommands()
        {
            c_StepCommand.RaiseCanExecuteChanged();
            c_PlayPauseCommand.RaiseCanExecuteChanged();
            c_RestartCommand.RaiseCanExecuteChanged();
            c_SimulationOptionsCommand.RaiseCanExecuteChanged();
            c_SimulateToCommand.RaiseCanExecuteChanged();
        }

        public int Step => simulator.Step;
        public double CenterSum => Math.Round(simulator.CenterSum, 9);
        public double EdgeSum => Math.Round(simulator.EdgeSum, 9);
        public int TotalSimTime => simulator.TotalSimTime;

        private string p_PlayPauseText;
        public string PlayPauseText
        {
            get { return p_PlayPauseText; }
            set
            {
                if (p_PlayPauseText != value)
                {
                    p_PlayPauseText = value; OnPropertyChanged("PlayPauseText");
                }
            }
        }

        private DelegateCommand c_PlayPauseCommand;
        public ICommand PlayPauseCommand => c_PlayPauseCommand;

        private DelegateCommand c_StepCommand;
        public ICommand StepCommand => c_StepCommand;

        private DelegateCommand c_RestartCommand;
        public ICommand RestartCommand => c_RestartCommand;

        private DelegateCommand c_SimulationOptionsCommand;
        public ICommand SimulationOptionsCommand => c_SimulationOptionsCommand;

        private DelegateCommand c_SimulateToCommand;
        public ICommand SimulateToCommand => c_SimulateToCommand;

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

        private VisualContext p_VisualContext;
        public VisualContext VisualContext
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
