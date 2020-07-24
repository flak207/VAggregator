using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KEA.VAggregator.WPF.Converters
{
    public class VideoQualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string retval = value?.ToString().TrimStart('_');
            if (retval.Contains("Unknown"))
                retval = "ALL";
            //else
            //    retval = retval.ToUpperInvariant();
            return retval;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
