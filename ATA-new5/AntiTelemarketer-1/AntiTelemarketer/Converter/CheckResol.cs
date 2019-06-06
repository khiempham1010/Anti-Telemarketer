using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public class CheckResol : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double _2kDesignFontSize = Double.Parse(parameter.ToString());

            var size = Plugin.XamJam.Screen.CrossScreen.Current.Size;


              return size.Height.ToString() + " - " + size.Width.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
