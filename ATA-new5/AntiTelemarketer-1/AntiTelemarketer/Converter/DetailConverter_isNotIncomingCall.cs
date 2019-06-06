using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public class DetailConverter_isNotIncomingCall : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() != "Incoming")
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
