namespace Monte_Carlo_Method_3D.ViewModels
{
    public enum CalculationMethod
    {
        Probability,
        Statistical
    }

    public class СlTabViewModel : TabViewModel
    {
        private CalculationMethod m_CalculationMethod;

        public СlTabViewModel() : base("Розрахунок")
        {
            
        }
    }
}