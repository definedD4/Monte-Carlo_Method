using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Calculation
{
    public enum CalculationMethod
    {
        [Description("Метод априорних вероятностей")]
        Propability,
        [Description("Метод статистических испытаний")]
        Statistical
    }

    class Calculation
    {
    }
}
