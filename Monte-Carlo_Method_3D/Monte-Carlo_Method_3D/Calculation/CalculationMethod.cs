using System.Collections.Generic;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Calculation
{
    public sealed class CalculationMethod
    {
        private CalculationMethod(string displayName, IEnumerable<CalculationConstraintCreator> availableConstraints)
        {
            DisplayName = displayName;
            AvailableConstraints = availableConstraints;
        }

        public string DisplayName { get; }

        public IEnumerable<CalculationConstraintCreator> AvailableConstraints { get; }

        private bool Equals(CalculationMethod other)
        {
            return string.Equals(DisplayName, other.DisplayName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is CalculationMethod && Equals((CalculationMethod) obj);
        }

        public override int GetHashCode()
        {
            return DisplayName?.GetHashCode() ?? 0;
        }

        public static bool operator ==(CalculationMethod left, CalculationMethod right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalculationMethod left, CalculationMethod right)
        {
            return !Equals(left, right);
        }

        public static CalculationMethod Propability => new CalculationMethod("ІПРАЙ", new[]
        {
            new CalculationConstraintCreator("За часом симуляції", "Час виконяння (мс):",
                arg => new CalculationConstraint(
                    info => ((PrSimulationInfo) info).TotalSimTime < double.Parse(arg),
                    $"Виконувати {double.Parse(arg)} мс.")),
            new CalculationConstraintCreator("За сумою в середені сітки",
                "Виконувати доки сума в середені сітки більша за:",
                arg => new CalculationConstraint(
                    info => ((PrSimulationInfo) info).CenterSum > double.Parse(arg),
                    $"Виконувати доки сума в середені сітки більша за {double.Parse(arg)}"))
        });

        public static CalculationMethod Statistical => new CalculationMethod("МСВ", new[]
        {
            new CalculationConstraintCreator("За часом симуляції", "Час виконяння (мс):",
                arg => new CalculationConstraint(
                    info => ((StSimulationInfo) info).TotalSimTime < double.Parse(arg),
                    $"Виконувати {double.Parse(arg)} мс.")),
            new CalculationConstraintCreator("За ітераціями", "Кількість ітерацій:",
                arg => new CalculationConstraint(
                    info => ((StSimulationInfo) info).TotalSimulations < long.Parse(arg),
                    $"Виконати {long.Parse(arg)} ітерацій."))
        });

        public static IEnumerable<CalculationMethod> AvailableMethods => new[]
        {
            Propability,
            Statistical
        };
    }
}
