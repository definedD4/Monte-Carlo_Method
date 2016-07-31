using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Monte_Carlo_Method_3D.GraphRendering;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class StatMethodViewModel : TabViewModel
    {
        private StatMethodSimulator m_Simulator;
        private StatMethodVisualizer m_Visualizer;

        // Property Backing Fields
        private bool m_SimulationInProgress = false;
        private StVisualContext m_VisualContext;

        //Commands
        private DelegateCommand m_StepCommand;

        public StatMethodViewModel() : base("Метод статистических испытаний")
        {
            m_Simulator = new StatMethodSimulator(5, 5, new IntPoint(2, 2));
            m_Visualizer = new StatMethodVisualizer(m_Simulator.Width, m_Simulator.Height, new HSVPallete());
            VisualContext = new StVisualContext2D(m_Simulator, m_Visualizer);

            VisualContext.UpdateVizualization();

            m_StepCommand = new DelegateCommand(_ =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateStep();
                m_VisualContext.UpdateVizualization();

                SimulationInProgress = false;
            }, _ => !SimulationInProgress);
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

        public ICommand StepCommand => m_StepCommand;
    }
}
