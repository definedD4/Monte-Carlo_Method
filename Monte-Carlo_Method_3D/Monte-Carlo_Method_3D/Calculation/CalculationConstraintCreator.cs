using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monte_Carlo_Method_3D.Util.AssertHelper;

namespace Monte_Carlo_Method_3D.Calculation
{
    public class CalculationConstraintCreator
    {
        private readonly Func<string, CalculationConstraint> m_Creator;

        public CalculationConstraintCreator(string displayName, string argumentDisplayName,
            Func<string, CalculationConstraint> creator)
        {
            displayName.AssertNotNull(nameof(displayName));
            argumentDisplayName.AssertNotNull(nameof(argumentDisplayName));

            DisplayName = displayName;
            ArgumentDisplayName = argumentDisplayName;
            m_Creator = creator;
        }

        public string DisplayName { get; }

        public string ArgumentDisplayName { get; }

        public CalculationConstraint Create(string argument) => m_Creator(argument);
    }
}
