using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Method_3D.Calculation
{
    public abstract class CalculationConstraintCreator
    {
        public abstract string ConstraintDisplayName { get; }

        public abstract string ConstraintArgumentDisplayName { get; }

        public abstract CalculationConstraint Create(object argument);
    }
}
