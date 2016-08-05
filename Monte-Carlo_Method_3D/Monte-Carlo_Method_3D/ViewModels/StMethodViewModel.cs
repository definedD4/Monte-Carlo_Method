using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using System.Windows.Threading;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StMethodViewModel : TabViewModel
    {
        private StSimulator m_Simulator;
        private StVisualizer m_Visualizer;

        private DispatcherTimer m_Timer;

        // Property Backing Fields
        private bool m_SimulationInProgress = false;
        private StVisualContext m_VisualContext;

        //Commands
        private DelegateCommand m_StepCommand;
        private SwitchStateCommand m_PlayPauseCommand;

        public StMethodViewModel() : base("Метод статистических испытаний")
        {
            m_Simulator = new StSimulator(5, 5, new IntPoint(2, 2));
            m_Visualizer = new StVisualizer(m_Simulator.Width, m_Simulator.Height, new HSVPallete());
            VisualContext = new StVisualContext2D(m_Simulator, m_Visualizer);

            VisualContext.UpdateVisualization();

            m_StepCommand = new DelegateCommand(_ =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateSteps(1);
                m_VisualContext.UpdateVisualization();

                SimulationInProgress = false;

                OnPropertyChanged(nameof(SimulationInfo));
            }, _ => !SimulationInProgress);

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

                    m_Simulator.SimulateSteps(100);
                    VisualContext.UpdateVisualization();

                    SimulationInProgress = false;

                    OnPropertyChanged(nameof(SimulationInfo));
                }
            };
        }

        private void UpdateCommands()
        {
            m_StepCommand.RaiseCanExecuteChanged();
            m_PlayPauseCommand.RaiseCanExecuteChanged();
        }

        public bool SimulationInProgress
        {
            get { return m_SimulationInProgress; }
            set { m_SimulationInProgress = value; OnPropertyChanged(nameof(m_SimulationInProgress)); m_StepCommand.RaiseCanExecuteChanged(); }
        }

        public StVisualContext VisualContext
        {
            get { return m_VisualContext; }
            set { m_VisualContext = value; OnPropertyChanged(nameof(VisualContext)); }
        }

        public StSimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        public ICommand StepCommand => m_StepCommand;

        public ICommand PlayPauseCommand => m_PlayPauseCommand;
    }
}
