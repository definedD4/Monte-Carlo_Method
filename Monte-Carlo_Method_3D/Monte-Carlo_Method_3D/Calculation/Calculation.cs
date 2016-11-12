using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Calculation
{
    public enum CalculationMethod
    {
        [Description("ІПРАЙ")]
        Propability,
        [Description("МСВ")]
        Statistical
    }

    public abstract class Calculation : INotifyPropertyChanged
    {
        private readonly ICalculationConstraint m_Constraint;
        private readonly BackgroundWorker m_Worker;
        private readonly IEnumerable<IntPoint> m_CalculationMask; 

        private int m_Progress;
        private GridData m_Result;

        public Calculation(ICalculationConstraint constraint, IEnumerable<IntPoint> calculationMask)
        {
            m_Constraint = constraint;
            m_CalculationMask = calculationMask;

            m_Worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            m_Worker.DoWork += (sender, args) =>
            {
                GridData result = Simulate(m_CalculationMask);
                if(result == null)
                {
                    args.Cancel = true;
                }
                args.Result = result;
            };
            m_Worker.ProgressChanged += (sender, args) =>
            {
                Progress = args.ProgressPercentage;
            };
            m_Worker.RunWorkerCompleted += (sender, args) =>
            {
                Result = (GridData)args.Result;

                DoneCalculation?.Invoke(this, EventArgs.Empty);
            };
        }

        protected abstract GridData Simulate(IEnumerable<IntPoint> mask);

        protected void ReportProgress(int progress)
        {
            m_Worker.ReportProgress(progress);
        }

        protected bool CanContinue(object simulationInfo)
        {
            return m_Constraint.CanContinue(simulationInfo);
        }

        protected bool CancelRequested()
        {
            return m_Worker.CancellationPending;
        }

        public void Start()
        {
            Progress = 0;
            m_Worker.RunWorkerAsync();
        }

        public void Cancel()
        {
            m_Worker.CancelAsync();
            Progress = 0;
        }

        public int Progress
        {
            get { return m_Progress; }
            set { m_Progress = value;  OnPropertyChanged(nameof(Progress));}
        }

        public GridData Result
        {
            get { return m_Result; }
            set { m_Result = value; OnPropertyChanged(nameof(Result)); }
        }

        public event EventHandler DoneCalculation;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Invalid property name.");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
