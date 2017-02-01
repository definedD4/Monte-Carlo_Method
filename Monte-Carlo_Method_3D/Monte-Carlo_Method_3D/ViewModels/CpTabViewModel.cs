using System;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class CpTabViewModel : TabViewModel
    {
        private PrSimulator m_PrSimulator;
        private StSimulator m_StSimulator;
        private DiffGenerator m_DiffGenerator;

        private Player m_Player;

        private Visualizer2D m_Visualizer;

        private IVisualization m_PrVisualization;
        private IVisualization m_StVisualization;
        private IVisualization m_DiffVisualization;

        public CpTabViewModel(SimulationOptions options) : base("Порівняння")
        {
            InitComponents(options);

            // Init palyer
            m_Player = new Player(() =>
            {
                Simulate(0.05d);
                
            }, () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    Simulate();
                }
            }, () =>
            {
                PrVisualization = m_Visualizer.GenerateTableVisualization(m_PrSimulator.GetData());
                StVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_StSimulator.GetData());
                DiffVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_DiffGenerator.GetData());

                OnPropertyChanged(nameof(PrSimulationInfo));
                OnPropertyChanged(nameof(StSimulationInfo));
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            PlayPauseCommand = new SwitchStateCommand("Пауза", "Програвання", false, _ => !m_Player.SingleStepRunning);
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

            SimulateStepCommand = new DelegateCommand(x =>
            {
                m_Player.StepOnce();
            }, x => !m_Player.RunningOrPlaying);

            RestartCommand = new DelegateCommand(x =>
            {
                m_PrSimulator.Reset();
                m_StSimulator.Reset();

                PrVisualization = m_Visualizer.GenerateTableVisualization(m_PrSimulator.GetData());
                StVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_StSimulator.GetData());
                DiffVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_DiffGenerator.GetData());

                OnPropertyChanged(nameof(PrSimulationInfo));
                OnPropertyChanged(nameof(StSimulationInfo));
            }, _ => !m_Player.RunningOrPlaying);

            SimulationOptionsCommand = new DelegateCommand(x =>
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

            m_Visualizer = new Visualizer2D();

            PrVisualization = m_Visualizer.GenerateTableVisualization(m_PrSimulator.GetData());
            StVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_StSimulator.GetData());
            DiffVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_DiffGenerator.GetData());
        }

        private void UpdateCommands()
        {
            PlayPauseCommand.RaiseCanExecuteChanged();
            RestartCommand.RaiseCanExecuteChanged();
            SimulationOptionsCommand.RaiseCanExecuteChanged();
        }

        public SwitchStateCommand PlayPauseCommand { get; }
        public DelegateCommand RestartCommand { get; }
        public DelegateCommand SimulationOptionsCommand { get; }
        public DelegateCommand SimulateStepCommand { get; }

        public IVisualization PrVisualization
        {
            get { return m_PrVisualization; }
            set { m_PrVisualization = value; OnPropertyChanged(nameof(PrVisualization));}
        }
        public IVisualization StVisualization
        {
            get { return m_StVisualization; }
            set { m_StVisualization = value; OnPropertyChanged(nameof(StVisualization)); }
        }
        public IVisualization DiffVisualization
        {
            get { return m_DiffVisualization; }
            set { m_DiffVisualization = value; OnPropertyChanged(nameof(DiffVisualization)); }
        }      

        public PrSimulationInfo PrSimulationInfo => m_PrSimulator.SimulationInfo;
        public StSimulationInfo StSimulationInfo => m_StSimulator.SimulationInfo;
    }
}
