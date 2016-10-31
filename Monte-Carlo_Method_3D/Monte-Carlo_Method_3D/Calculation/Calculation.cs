using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Calculation
{
    public enum CalculationMethod
    {
        [Description("Метод априорних вероятностей")]
        Propability,
        [Description("Метод статистических испытаний")]
        Statistical
    }

    public abstract class Calculation : INotifyPropertyChanged
    {
        private readonly ICalculationConstraint m_Constraint;
        private readonly BackgroundWorker m_Worker;

        private int m_Progress;
        private double[,] m_Result;

        public Calculation(ICalculationConstraint constraint)
        {
            m_Constraint = constraint;

            m_Worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            m_Worker.DoWork += (sender, args) =>
            {
                double[,] result = Simulate();
                args.Result = result;
            };
            m_Worker.ProgressChanged += (sender, args) =>
            {
                Progress = args.ProgressPercentage;
            };
            m_Worker.RunWorkerCompleted += (sender, args) =>
            {
                Result = (double[,]) args.Result;

                DoneCalculation?.Invoke(this, EventArgs.Empty);
            };
        }

        protected abstract double[,] Simulate();

        protected void ReportProgress(int progress)
        {
            m_Worker.ReportProgress(progress);
        }

        protected bool CanContinue(object simulationInfo)
        {
            return m_Constraint.CanContinue(simulationInfo);
        }

        public int Progress
        {
            get { return m_Progress; }
            set { m_Progress = value;  OnPropertyChanged(nameof(Progress));}
        }

        public double[,] Result
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
