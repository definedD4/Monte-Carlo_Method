using Monte_Carlo_Method_3D.Simulation;
using System;

namespace Monte_Carlo_Method_3D.Calculation
{
    public interface ICalculationConstraint
    {
        bool CanContinue(object simulationInfo);
    }

    public class PrCenterSumCalcConstraint : ICalculationConstraint
    {
        private double m_Limit;

        public PrCenterSumCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is PrSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as PrSimulationInfo).CenterSum > m_Limit;
        }
    }

    public class PrSimTimeCalcConstraint : ICalculationConstraint
    {
        private double m_Limit;

        public PrSimTimeCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is PrSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as PrSimulationInfo).TotalSimTime < m_Limit;
        }
    }

    public class StSimTimeCalcConstraint : ICalculationConstraint
    {
        private double m_Limit;

        public StSimTimeCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is StSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as StSimulationInfo).TotalSimTime < m_Limit;
        }
    }

    public class StStepsCalcConstraint : ICalculationConstraint
    {
        private long m_Limit;

        public StStepsCalcConstraint(long limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is StSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as StSimulationInfo).TotalSimulations < m_Limit;
        }
    }
}