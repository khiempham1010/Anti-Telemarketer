using System;
using System.Globalization;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public class ToMinuteConverter : IValueConverter
    {
        private string TakeMinute(ref int second)
        {
            string min = (second / 60).ToString();
            second = second % 60;
            return min;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int second = Int32.Parse(value.ToString());
            string minute = TakeMinute(ref second);
            int _minute = Int32.Parse(minute);
            string hour = TakeMinute(ref _minute);
            minute = _minute.ToString();


            string timeFormat = string.Empty;
            if (!hour.Equals("0"))
            {
                timeFormat += hour;
                timeFormat = timeFormat + ":";
            }


            if (_minute < 10)
            {
                timeFormat = timeFormat + "0" + minute;
            }
            else
            {
                timeFormat = timeFormat + minute;
            }

            timeFormat = timeFormat + ":";

            if (second < 10)
            {
                timeFormat = timeFormat + "0" + second.ToString();
            }
            else
            {
                timeFormat = timeFormat + second.ToString();
            }

            return timeFormat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
