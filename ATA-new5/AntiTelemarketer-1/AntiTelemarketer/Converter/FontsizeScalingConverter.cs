using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public class FontsizeScalingConverter: IValueConverter
    {
        public FontsizeScalingConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double _2kDesignFontSize = Double.Parse(parameter.ToString());

            var size = Plugin.XamJam.Screen.CrossScreen.Current.Size;


         //   return size.Height.ToString() + " - " + size.Width.ToString();

            //4K
            if (size.Height*2 > 3000)
                return _2kDesignFontSize * 2.3;
            //2K
            if (size.Height*2 > 2400)
                return _2kDesignFontSize * 1.7;
            //FULL HD
            if (size.Height*2 > 1700)
                return _2kDesignFontSize * 1.5;
            //HD
            if(size.Height*2 >= 900)
                return _2kDesignFontSize / 1.5;

            //WVGA
            return _2kDesignFontSize / 8;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
