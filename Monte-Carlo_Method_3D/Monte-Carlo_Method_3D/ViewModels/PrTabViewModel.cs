﻿using Monte_Carlo_Method_3D.Dialogs;
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
    public class PrTabViewModel : TabViewModel
    {
        private PrSimulator m_Simulator;
        private PrVisualizer m_Visualizer;
        private Pallete m_Pallete;
        private DispatcherTimer m_Timer;
        private PrVisualContext m_VisualContext;
        private GaugeContext m_Gauge;

        private bool m_SimulationInProgress = false;
        private long m_SimulateToStep;

        private readonly SwitchStateCommand m_PlayPauseCommand;
        private readonly DelegateCommand m_StepCommand;
        private readonly DelegateCommand m_RestartCommand;
        private readonly DelegateCommand m_SimulationOptionsCommand;
        private readonly DelegateCommand m_SimulateToCommand;
        private readonly DelegateCommand m_ExportToCsvCommand;

        public PrTabViewModel(SimulationOptions options) : base("Метод вероятностей")
        {
            m_Pallete = new Pallete();

            Gauge = new GaugeContext(m_Pallete);

            InitComponents(options);

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

            m_StepCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateSteps();
                VisualContext.UpdateVisualization();

                SimulationInProgress = false;
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

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
                m_Simulator.Reset();
                VisualContext.UpdateVisualization();
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            m_SimulationOptionsCommand = new DelegateCommand(x =>
            {
                SimulationOptionsDialog dialog = new SimulationOptionsDialog(SimulationOptions.FromSimulator(m_Simulator));
                dialog.ShowDialog();

                if (dialog.DialogResult.GetValueOrDefault(false))
                {
                    SimulationOptions result = dialog.SimulationOptions;
                    InitComponents(result);
                }
            }, x => !(SimulationInProgress || m_Timer.IsEnabled));

            m_SimulateToCommand = new DelegateCommand(x =>
            {
                SimulationInProgress = true;

                m_Simulator.SimulateSteps(Math.Max(SimulateToStep - m_Simulator.Step, 0L));

                VisualContext.UpdateVisualization();

                SimulationInProgress = false;
                OnPropertyChanged(nameof(SimulationInfo));
            }, x => !(SimulationInProgress || m_Timer.IsEnabled || SimulateToStep <= 0));

            m_ExportToCsvCommand = new DelegateCommand(x =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    new CsvExporter(';').ExportToFile(m_Simulator.GetData(), saveFileDialog.FileName);
                }
            });

            VisualTypeSelector = new SelectorCommand("2D");

            VisualTypeSelector.SelectionChanged += (s, e) =>
            {
                if(VisualTypeSelector.SelectedValue == "2D")
                {
                    VisualContext = new PrVisualContext2D(m_Simulator, m_Visualizer);
                }
                else if(VisualTypeSelector.SelectedValue == "3D")
                {
                    VisualContext = new PrVisualContext3D(m_Simulator, m_Visualizer);
                }
                else if (VisualTypeSelector.SelectedValue == "Table")
                {
                    VisualContext = new PrTableVisualContext(m_Simulator, m_Visualizer);
                }
                VisualContext.UpdateVisualization();
            };
            VisualTypeSelector.RaiseSelectionChanged();
            VisualTypeSelector.UpdateSelectors();
        }

        private void InitComponents(SimulationOptions options)
        {
            m_Simulator = new PrSimulator(options);
            m_Visualizer = new PrVisualizer(m_Simulator, m_Pallete);
            VisualTypeSelector?.RaiseSelectionChanged();
            OnPropertyChanged(nameof(SimulationInfo));
        }

        private void UpdateCommands()
        {
            m_StepCommand.RaiseCanExecuteChanged();
            m_PlayPauseCommand.RaiseCanExecuteChanged();
            m_RestartCommand.RaiseCanExecuteChanged();
            m_SimulationOptionsCommand.RaiseCanExecuteChanged();
            m_SimulateToCommand.RaiseCanExecuteChanged();
            m_ExportToCsvCommand.RaiseCanExecuteChanged();
        }

        public bool SimulationInProgress
        {
            get { return m_SimulationInProgress; }
            private set { m_SimulationInProgress = value; OnPropertyChanged(nameof(SimulationInProgress)); UpdateCommands(); }
        }

        public PrSimulationInfo SimulationInfo => m_Simulator.SimulationInfo;

        public ICommand PlayPauseCommand => m_PlayPauseCommand;
        public ICommand StepCommand => m_StepCommand;
        public ICommand RestartCommand => m_RestartCommand;
        public ICommand SimulationOptionsCommand => m_SimulationOptionsCommand;
        public ICommand SimulateToCommand => m_SimulateToCommand;
        public ICommand ExportToCsvCommand => m_ExportToCsvCommand;

        public long SimulateToStep
        {
            get { return m_SimulateToStep; }
            set { m_SimulateToStep = value; OnPropertyChanged("SimuateToStep"); m_SimulateToCommand.RaiseCanExecuteChanged(); }
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
