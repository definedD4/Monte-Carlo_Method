using System;
using System.Windows.Data;
using static System.Math;

namespace Monte_Carlo_Method_3D.Util
{
    public static class FormattingUtil
    {
        public static string FormatShort(double value)
        {
            if (value == 0 || Round(value, 4) > 0)
            {
                return Round(value, 4).ToString();
            }
            else
            {
                return value.ToString("G2");
            }
        }

        public static string FormatLong(double value)
        {
            if (value == 0 || Round(value, 6) > 0)
            {
                return Round(value, 6).ToString();
            }
            else
            {
                return value.ToString("E6");
            }
        }
    }

    public class ShortFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentException();

            return FormattingUtil.FormatShort((double) value);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class LongFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentException();

            return FormattingUtil.FormatLong((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}