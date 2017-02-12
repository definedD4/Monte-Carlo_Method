using System;
using Monte_Carlo_Method_3D.Util.AssertHelper;

namespace Monte_Carlo_Method_3D.Calculation
{
    public class CalculationConstraint
    {
        private readonly Func<object, bool> m_Predicate;

        public string Description { get; }

        public CalculationConstraint(Func<object, bool> predicate, string description)
        {
            description.AssertNotNullOrWhitespace(nameof(description));

            m_Predicate = predicate;
            Description = description;
        }

        public bool CanContinue(object simulationInfo)
        {
            simulationInfo.AssertNotNull(nameof(simulationInfo));

            return m_Predicate(simulationInfo);
        }

        public override string ToString() => Description;
    }
}