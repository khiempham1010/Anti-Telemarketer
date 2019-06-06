using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
 // THIS CLASS USE FOR DETECT DO WE VISIBLE INCOMING CALL
    public class DetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Incoming")
                return true;
            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
