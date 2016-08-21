using System;
using System.Windows.Input;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Gauge;
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
        private DispatcherTimer m_Timer;
        private GaugeContext m_Gauge;

        private PrVisualizer m_PrVisualizer;
        private StVisualizer m_StVisualizer;
        private DiffVisualizer m_DiffVisualizer;

        private PrVisualContext m_PrVisualContext;
        private StVisualContext m_StVisualContext;
        private DiffVisualContext m_DiffVisualContext;

        private bool m_SimulationInProgress = false;

        private SwitchStateCommand m_PlayPauseCommand;
        private DelegateCommand m_RestartCommand;
        private DelegateCommand m_SimulationOptionsCommand;

        public CpTabViewModel() : base("Сравнение")
        {
            m_Pallete = new Pallete();

            m_Gauge = new GaugeContext(m_Pallete);

            Init(5, 5, new IntPoint(3, 3));

            m_Timer = new DispatcherTimer(DispatcherPriority.ContextIdle) { Interval = TimeSpan.FromMilliseconds(10) };
            m_Timer.Tick += (s, e) =>
            {
                if (!SimulationInProgress)
                {
                    SimulationInProgress = true;

                    for (int i = 0; i < 50; i++)
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

                    m_PrVisualContext.UpdateVisualization();
                    m_StVisualContext.UpdateVisualization();
                    m_DiffVisualContext.UpdateVisualization();

                    SimulationInProgress = false;

                    OnPropertyChanged(nameof(PrSimulationInfo));
                    OnPropertyChanged(nameof(StSimulationInfo));
                }
            };

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

            m_RestartCommand = new DelegateCommand(x =>
            {
                m_PrSimulator.Reset();
                m_StSimulator.Reset();

                m_PrVisualContext.UpdateVisualization();
                m_StVisualContext.UpdateVisualization();
                m_DiffVisualContext.UpdateVisualization();

                OnPropertyChanged(nameof(PrSimulationInfo));
                OnPropertyChanged(nameof(StSimulationInfo));
            }, _ => !(SimulationInProgress || m_Timer.IsEnabled));

            m_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_PrSimulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    Init(result.Width, result.Height, result.StartLocation);
                    OnPropertyChanged(nameof(PrSimulationInfo));
                    OnPropertyChanged(nameof(StSimulationInfo));
                }
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));
        }

        private void Init(int width, int height, IntPoint startLocation)
        {
            m_PrSimulator = new PrSimulator(width, height, startLocation);
            m_StSimulator = new StSimulator(width, height, startLocation);
            m_DiffGenerator = new DiffGenerator(m_PrSimulator, m_StSimulator);


            m_PrVisualizer = new PrVisualizer(m_PrSimulator, m_Pallete);
            m_StVisualizer = new StVisualizer(m_StSimulator, m_Pallete);
            m_DiffVisualizer = new DiffVisualizer(m_DiffGenerator);

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

        public GaugeContext Gauge
        {
            get { return m_Gauge; }
            set { m_Gauge = value; OnPropertyChanged(nameof(Gauge)); }
        }

        public PrSimulationInfo PrSimulationInfo => m_PrSimulator.SimulationInfo;
        public StSimulationInfo StSimulationInfo => m_StSimulator.SimulationInfo;
    }
}
