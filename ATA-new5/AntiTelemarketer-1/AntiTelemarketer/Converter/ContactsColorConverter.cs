using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public class ContactsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals("Missed"))
                return Color.Red;
            if (value.ToString().Equals("Incoming"))
                return Color.Green;
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
