using Monte_Carlo_Method_3D.Calculation;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class СlTabViewModel : TabViewModel
    {
        private CalculationMethod m_CalculationMethod;
        private ConstraintChooserViewModel m_ConstraintChooser;

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
                m_ConstraintChooser = new ConstraintChooserViewModel(m_CalculationMethod);
                OnPropertyChanged(nameof(CalculationMethod));
            }
        }

        public ConstraintChooserViewModel ConstraintChooser
        {
            get { return m_ConstraintChooser; }
            set { m_ConstraintChooser = value; OnPropertyChanged(nameof(ConstraintChooser)); }
        }
    }
}