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
        private readonly double m_Limit;

        public PrCenterSumCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is PrSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as PrSimulationInfo).CenterSum > m_Limit;
        }

        public override string ToString() => $"Simulate while center sum is greater than: {m_Limit}.";
    }

    public class PrSimTimeCalcConstraint : ICalculationConstraint
    {
        private readonly double m_Limit;

        public PrSimTimeCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is PrSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as PrSimulationInfo).TotalSimTime < m_Limit;
        }

        public override string ToString() => $"Simulate for: {m_Limit} ms.";
    }

    public class StSimTimeCalcConstraint : ICalculationConstraint
    {
        private readonly double m_Limit;

        public StSimTimeCalcConstraint(double limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is StSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as StSimulationInfo).TotalSimTime < m_Limit;
        }

        public override string ToString() => $"Simulate for: {m_Limit} ms.";
    }

    public class StStepsCalcConstraint : ICalculationConstraint
    {
        private readonly long m_Limit;

        public StStepsCalcConstraint(long limit)
        {
            m_Limit = limit;
        }

        public bool CanContinue(object simulationInfo)
        {
            if (!(simulationInfo is StSimulationInfo)) throw new ArgumentException();
            return (simulationInfo as StSimulationInfo).TotalSimulations < m_Limit;
        }

        public override string ToString() => $"Simulate for: {m_Limit} steps.";
    }
}