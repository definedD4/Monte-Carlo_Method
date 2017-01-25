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

        private Pallete m_Pallete;
        private Player m_Player;

        private PrVisualizer m_PrVisualizer;
        private StVisualizer m_StVisualizer;
        private DiffVisualizer m_DiffVisualizer;

        private IVisualization m_PrVisualization;
        private IVisualization m_StVisualization;
        private IVisualization m_DiffVisualization;

        public CpTabViewModel(SimulationOptions options) : base("Порівняння")
        {
            m_Pallete = new Pallete();

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
                PrVisualization = m_PrVisualizer.GenerateTableVisualization(m_PrSimulator.GetData());
                StVisualization = m_StVisualizer.GenerateTableVisualization(m_StSimulator.GetData());
                DiffVisualization = m_DiffVisualizer.GenerateTableVisualization(m_DiffGenerator.GetData());

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

                PrVisualization = m_PrVisualizer.GenerateTableVisualization(m_PrSimulator.GetData());
                StVisualization = m_StVisualizer.GenerateTableVisualization(m_StSimulator.GetData());
                DiffVisualization = m_DiffVisualizer.GenerateTableVisualization(m_DiffGenerator.GetData());

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


            m_PrVisualizer = new PrVisualizer(m_PrSimulator.Size, m_Pallete);
            m_StVisualizer = new StVisualizer(m_StSimulator.Size, m_StSimulator.StartLocation, m_Pallete);
            m_DiffVisualizer = new DiffVisualizer(m_DiffGenerator.Size);

            PrVisualization = m_PrVisualizer.GenerateTableVisualization(m_PrSimulator.GetData());
            StVisualization = m_StVisualizer.GenerateTableVisualization(m_StSimulator.GetData());
            DiffVisualization = m_DiffVisualizer.GenerateTableVisualization(m_DiffGenerator.GetData());
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
