using System;
using System.Reactive;
using System.Windows.Threading;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Util.Commands;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class CpTabViewModel : TabViewModel
    {
        private readonly Player m_Player;

        private PrSimulator m_PrSimulator;
        private StSimulator m_StSimulator;
        private DiffGenerator m_DiffGenerator;
        
        private Visualizer2D m_Visualizer;

        [Reactive]
        public IVisualization PrVisualization { get; private set; }

        [Reactive]
        public IVisualization StVisualization { get; private set; }

        [Reactive]
        public IVisualization DiffVisualization { get; private set; }

        [Reactive]
        public PrSimulationInfo PrSimulationInfo { get; private set; }

        [Reactive]
        public StSimulationInfo StSimulationInfo { get; private set; }

        private ReactiveCommand<Unit, Unit> UpdateVisualization { get; }

        public ReactiveCommand<Unit, Unit> SimulateStep { get; }

        public ReactivePlayPauseCommand PlayPause { get; }
        
        public ReactiveCommand<Unit, Unit> Restart { get; }

        public ReactiveCommand<Unit, Unit> OpenSimualationOptions { get;}

        public CpTabViewModel(SimulationOptions options) : base("Порівняння")
        {
            InitComponents(options);

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
                UpdateVisualization.Execute().Subscribe();
            }, TimeSpan.FromMilliseconds(10), DispatcherPriority.ContextIdle);

            UpdateVisualization = ReactiveCommand.Create(() =>
            {
                PrVisualization = m_Visualizer.GenerateTableVisualization(m_PrSimulator.GetData());
                StVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_StSimulator.GetData());
                DiffVisualization = m_Visualizer.GenerateEdgeTableVisualization(m_DiffGenerator.GetData());

                PrSimulationInfo = m_PrSimulator.SimulationInfo;
                StSimulationInfo = m_StSimulator.SimulationInfo;
            });

            var notRunningOrPlaying = m_Player.WhenAny(x => x.RunningOrPlaying, r => !r.Value);

            var notSingleStepRunning = m_Player.WhenAny(x => x.SingleStepRunning, r => !r.Value);

            SimulateStep = ReactiveCommand.Create(() =>
            {
                m_Player.StepOnce();
            }, notRunningOrPlaying);

            PlayPause = new ReactivePlayPauseCommand("Програти", "Пауза", notSingleStepRunning);

            PlayPause.Subscribe(p =>
            {
                if (p)
                {
                    m_Player.Start();
                }
                else
                {
                    m_Player.Stop();
                }
            });

            Restart = ReactiveCommand.Create(() =>
            {
                m_PrSimulator.Reset();
                m_StSimulator.Reset();
            }, notRunningOrPlaying);

            Restart.InvokeCommand(UpdateVisualization);

            OpenSimualationOptions = ReactiveCommand.Create(() =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_PrSimulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    InitComponents(result);
                }
            }, notRunningOrPlaying);

            OpenSimualationOptions.InvokeCommand(UpdateVisualization);

            UpdateVisualization.Execute().Subscribe();
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
        }
    }
}
