using Monte_Carlo_Method_3D.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class ConstraintChooserViewModel : ViewModelBase
    {
        public class ConstraintCreator
        {
            private readonly Func<string, ICalculationConstraint> m_Creator;
            private readonly string m_ReadableName;
            private readonly string m_ArgName;
            private readonly CalculationMethod m_CalculationMethod;

            public ConstraintCreator(Func<string, ICalculationConstraint> creator, string readableName, string argName, CalculationMethod calculationMethod)
            {
                m_Creator = creator;
                m_ReadableName = readableName;
                m_ArgName = argName;
                m_CalculationMethod = calculationMethod;
            }

            public ICalculationConstraint Create(string arg) => m_Creator(arg);
            public override string ToString() => m_ReadableName;

            public string ReadableName => m_ReadableName;
            public string ArgName => m_ArgName;
            public CalculationMethod CalculationMethod => m_CalculationMethod;
        }

        private static readonly IEnumerable<ConstraintCreator> ConstraintCreators = new[] {
            new ConstraintCreator((x) => new PrCenterSumCalcConstraint(double.Parse(x)), "По сумме в центре", "Выполнять пока сумма в центре больше:", CalculationMethod.Propability),
            new ConstraintCreator((x) => new PrSimTimeCalcConstraint(double.Parse(x)), "По времени симуляции", "Время выполнения (мс):", CalculationMethod.Propability),
            new ConstraintCreator((x) => new StSimTimeCalcConstraint(double.Parse(x)), "По времени симуляции", "Время выполнения (мс):", CalculationMethod.Statistical),
            new ConstraintCreator((x) => new StStepsCalcConstraint(long.Parse(x)), "По итерациям", "Кол-во итераций:", CalculationMethod.Statistical)
        };

        private readonly CalculationMethod m_CalculationMethod;

        private ConstraintCreator m_CurrentConstraintCreator;
        private string m_CreatorArg;

        public ConstraintChooserViewModel(CalculationMethod calculationMethod)
        {
            m_CalculationMethod = calculationMethod;

            CurrentConstraintCreator = Creators.First();
        }

        public IEnumerable<ConstraintCreator> Creators => ConstraintCreators.Where(i => i.CalculationMethod == m_CalculationMethod);
        public ConstraintCreator CurrentConstraintCreator
        {
            get { return m_CurrentConstraintCreator; }
            set
            {
                m_CurrentConstraintCreator = value;
                OnPropertyChanged(nameof(CurrentConstraintCreator));
                OnPropertyChanged(nameof(ArgName));
            }
        }

        public string CreatorArg
        {
            get { return m_CreatorArg; }
            set { m_CreatorArg = value; OnPropertyChanged(nameof(CreatorArg)); }
        }

        public string ArgName => CurrentConstraintCreator?.ArgName;

        public ICalculationConstraint GetConstraint()
        {
            return m_CurrentConstraintCreator.Create(m_CreatorArg);
        }
    }
}
