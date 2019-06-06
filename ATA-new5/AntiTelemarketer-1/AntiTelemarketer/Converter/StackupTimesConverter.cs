using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public class StackupTimesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(Int16.Parse(value.ToString()) == 1)
            {
                return false;
            }
            return true;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
