using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.ViewModels
{
    // TODO: Get rid of generic. Find a way to generalize constraint
    public interface ICalculationConstraint<T>
    {
        bool CanContinue(T simulationInfo);
    }

    public class PrCenterSumCalcConstraint : ICalculationConstraint<PrSimulationInfo>
    {
        private double m_Limit;

        public PrCenterSumCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(PrSimulationInfo simulationInfo)
        {
            return simulationInfo.CenterSum > m_Limit;
        }
    }

    public class PrSimTimeCalcConstraint : ICalculationConstraint<PrSimulationInfo>
    {
        private double m_Limit;

        public PrSimTimeCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(PrSimulationInfo simulationInfo)
        {
            return simulationInfo.TotalSimTime < m_Limit;
        }
    }

    public class StSimTimeCalcConstraint : ICalculationConstraint<StSimulationInfo>
}