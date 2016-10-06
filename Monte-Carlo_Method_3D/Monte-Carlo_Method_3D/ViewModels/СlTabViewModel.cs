using Monte_Carlo_Method_3D.Calculation;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class СlTabViewModel : TabViewModel
    {
        private enum TState
        {
            NotStarted,
            Working,
            Paused
        }

        private CalculationMethod m_CalculationMethod;
        private ConstraintChooserViewModel m_ConstraintChooser;
        private TState m_State;
        private string m_OutputPath;

        public СlTabViewModel() : base("Розрахунок")
        {
            CalculationMethod = CalculationMethod.Propability;
        }

        public CalculationMethod CalculationMethod
        {
            get { return m_CalculationMethod; }
            set
            {
                m_CalculationMethod = value;
                ConstraintChooser = new ConstraintChooserViewModel(m_CalculationMethod);
                OnPropertyChanged(nameof(CalculationMethod));
            }
        }

        public ConstraintChooserViewModel ConstraintChooser
        {
            get { return m_ConstraintChooser; }
            set { m_ConstraintChooser = value; OnPropertyChanged(nameof(ConstraintChooser)); }
        }

        private TState State
        {
            get { return m_State; }
            set
            {
                m_State = value;
            }
        }


        public string OutputPath
        {
            get { return m_OutputPath; }
            set { m_OutputPath = value;  OnPropertyChanged(nameof(OutputPath)); }
        }


    }
}