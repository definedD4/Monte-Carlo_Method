using System;
using System.Windows.Input;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class CpTabViewModel : TabViewModel
    {
        private PrSimulator m_PrSimulator;
        private StSimulator m_StSimulator;
        private DiffGenerator m_DiffGenerator;

        private Pallete m_Pallete;
        private Player m_Player;

        private PrVisualizer m_PrVisualizer;
        private StVisualizer m_StVisualizer;
        private DiffVisualizer m_DiffVisualizer;

        private PrVisualContext m_PrVisualContext;
        private StVisualContext m_StVisualContext;
        private DiffVisualContext m_DiffVisualContext;

        private bool m_SimulationInProgress = false;

        private readonly SwitchStateCommand m_PlayPauseCommand;
        private readonly DelegateCommand m_RestartCommand;
        private readonly DelegateCommand m_SimulationOptionsCommand;
        private readonly DelegateCommand m_SimulateStepCommand;

        public CpTabViewModel(SimulationOptions options) : base("Порівняння")
        {
            m_Pallete = new Pallete();

            InitComponents(options);

            // Init palyer
            m_Player = new Player(() =>
            {
                Simulate(0.05d);

                m_PrVisualContext.UpdateVisualization();
                m_StVisualContext.UpdateVisualization();
                m_DiffVisualContext.UpdateVisualization();
            }, () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    Simulate();
                }

                m_PrVisualContext.UpdateVisualization();
                m_StVisualContext.UpdateVisualization();
                m_DiffVisualContext.UpdateVisualization();
            }, () =>
            {
                OnPropertyChanged(nameof(PrSimulationInfo));
                OnPropertyChanged(nameof(StSimulationInfo));
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            m_PlayPauseCommand = new SwitchStateCommand("Пауза", "Програвання", false, _ => !m_Player.SingleStepRunning);
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

            m_SimulateStepCommand = new DelegateCommand(x =>
            {
                m_Player.StepOnce();
            }, x => !m_Player.RunningOrPlaying);

            m_RestartCommand = new DelegateCommand(x =>
            {
                m_PrSimulator.Reset();
                m_StSimulator.Reset();

                m_PrVisualContext.UpdateVisualization();
                m_StVisualContext.UpdateVisualization();
                m_DiffVisualContext.UpdateVisualization();

                OnPropertyChanged(nameof(PrSimulationInfo));
                OnPropertyChanged(nameof(StSimulationInfo));
            }, _ => !m_Player.RunningOrPlaying);

            m_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_PrSimulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    InitComponents(result);
                    OnPropertyChanged(nameof(PrSimulationInfo));
                    OnPropertyChanged(nameof(StSimulationInfo));
                }
            }, x => !m_Player.RunningOrPlaying);
        }

        private void Simulate(double time = 0d)
        {
            double prStartSimTime = m_PrSimulator.TotalSimTime;
            double stStartSimTime = m_StSimulator.TotalSimTime;

            do
            {
                if (Math.Abs(m_PrSimulator.SimulationInfo.CenterSum) < 1e-100)
                {
                    m_StSimulator.SimulateSteps(200);
                }
                else
                {
                    if (m_PrSimulator.TotalSimTime < m_StSimulator.TotalSimTime)
                    {
                        m_PrSimulator.SimulateSteps(1);
                    }
                    else
                    {
                        m_StSimulator.SimulateSteps(1);
                    }
                }
            } while (m_PrSimulator.TotalSimTime - prStartSimTime < time &&
                     m_StSimulator.TotalSimTime - stStartSimTime < time);
        }

        private void InitComponents(SimulationOptions options)
        {
            m_PrSimulator = new PrSimulator(options);
            m_StSimulator = new StSimulator(options);
            m_DiffGenerator = new DiffGenerator(m_PrSimulator, m_StSimulator);


            m_PrVisualizer = new PrVisualizer(m_PrSimulator.Size, m_Pallete);
            m_StVisualizer = new StVisualizer(m_StSimulator.Size, m_StSimulator.StartLocation, m_Pallete);
            m_DiffVisualizer = new DiffVisualizer(m_DiffGenerator.Size);

            PrVisualContext = new PrTableVisualContext(m_PrSimulator, m_PrVisualizer);
            StVisualContext = new StTableVisualContext(m_StSimulator, m_StVisualizer);
            DiffVisualContext = new DiffVisualContext(m_DiffGenerator, m_DiffVisualizer);

            PrVisualContext.UpdateVisualization();
            StVisualContext.UpdateVisualization();
            DiffVisualContext.UpdateVisualization();
        }

        private void UpdateCommands()
        {
            m_PlayPauseCommand.RaiseCanExecuteChanged();
            m_RestartCommand.RaiseCanExecuteChanged();
            m_SimulationOptionsCommand.RaiseCanExecuteChanged();
        }

        public bool SimulationInProgress
        {
            get { return m_SimulationInProgress; }
            private set { m_SimulationInProgress = value; OnPropertyChanged(nameof(SimulationInProgress)); UpdateCommands(); }
        }

        public ICommand PlayPauseCommand => m_PlayPauseCommand;
        public ICommand RestartCommand => m_RestartCommand;
        public ICommand SimulationOptionsCommand => m_SimulationOptionsCommand;
        public ICommand SimulateStepCommand => m_SimulateStepCommand;

        public PrVisualContext PrVisualContext
        {
            get { return m_PrVisualContext; }
            set { m_PrVisualContext = value; OnPropertyChanged(nameof(PrVisualContext));}
        }
        public StVisualContext StVisualContext
        {
            get { return m_StVisualContext; }
            set { m_StVisualContext = value; OnPropertyChanged(nameof(StVisualContext)); }
        }
        public DiffVisualContext DiffVisualContext
        {
            get { return m_DiffVisualContext; }
            set { m_DiffVisualContext = value; OnPropertyChanged(nameof(DiffVisualContext)); }
        }      

        public PrSimulationInfo PrSimulationInfo => m_PrSimulator.SimulationInfo;
        public StSimulationInfo StSimulationInfo => m_StSimulator.SimulationInfo;
    }
}
