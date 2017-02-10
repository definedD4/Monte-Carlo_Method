using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using JetBrains.Annotations;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util.AssertHelper;

namespace Monte_Carlo_Method_3D.Calculation
{
    public abstract class Calculation
    {
        private readonly CalculationConstraint m_Constraint;
        private readonly BackgroundWorker m_Worker;
        private readonly IEnumerable<GridIndex> m_CalculationMask;
        private readonly Subject<double> m_Progress = new Subject<double>();
        private readonly Subject<GridData> m_Result = new Subject<GridData>();

        public Calculation([NotNull] CalculationConstraint constraint, IEnumerable<GridIndex> calculationMask)
        {
            constraint.AssertNotNull(nameof(constraint));
            
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
                m_Progress.OnNext(args.ProgressPercentage);
            };
            m_Worker.RunWorkerCompleted += (sender, args) =>
            {
                if (!args.Cancelled)
                {
                    m_Result.OnNext((GridData) args.Result);
                    m_Result.OnCompleted();
                    m_Progress.OnCompleted();
                }
            };
        }

        protected abstract GridData Simulate(IEnumerable<GridIndex> mask);

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
            m_Progress.OnNext(0d);
            m_Worker.RunWorkerAsync();
        }

        public void Cancel()
        {
            m_Worker.CancelAsync();
            m_Progress.OnNext(0d);
            m_Progress.OnCompleted();
            m_Result.OnCompleted();
        }

        public IObservable<double> Progress => m_Progress;

        public IObservable<GridData> Result => m_Result;
    }
}
