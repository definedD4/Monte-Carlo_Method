using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Method_3D.Calculation
{
    public sealed class CalculationMethod
    {
        private CalculationMethod(string displayName, IEnumerable<CalculationConstraint> alailableConstraints)
        {
            DisplayName = displayName;
            AlailableConstraints = alailableConstraints;
        }

        public string DisplayName { get; }

        // TODO: Switch to constraint creator or do somethiing with constraint
        public IEnumerable<CalculationConstraint> AlailableConstraints { get; }

        public static CalculationMethod Propability => new CalculationMethod("ІПРАЙ", new CalculationConstraint[] {});

        public static CalculationMethod Statistical => new CalculationMethod("МСВ", new CalculationConstraint[] { });

        public static IEnumerable<CalculationMethod> AvailbleMethods => new[] {Propability, Statistical};

        protected bool Equals(CalculationMethod other)
        {
            return string.Equals(DisplayName, other.DisplayName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CalculationMethod) obj);
        }

        public override int GetHashCode()
        {
            return (DisplayName != null ? DisplayName.GetHashCode() : 0);
        }
    }
}
