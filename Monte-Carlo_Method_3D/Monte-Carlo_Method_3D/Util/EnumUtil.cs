using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Monte_Carlo_Method_3D.Util
{
    public class EnumSelection : MarkupExtension
    {
        private readonly Type m_type;

        public EnumSelection(Type type)
        {
            m_type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(m_type)
                .Cast<object>()
                .Select(e => new { Value = (Enum)e, DisplayName = ((Enum)e).Description() });
        }

    }

    public static class EnumExtension
    {
        public static string Description(this Enum eValue)
        {
            var nAttributes = eValue.GetType().GetField(eValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (nAttributes.Any())
                return (nAttributes.First() as DescriptionAttribute).Description;

            TextInfo oTI = CultureInfo.CurrentCulture.TextInfo;
            return oTI.ToTitleCase(oTI.ToLower(eValue.ToString().Replace("_", " ")));
        }
    }
}
