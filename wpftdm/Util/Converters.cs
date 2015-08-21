using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace wpftdm.Util
{
    public class SecondsToTimeStringConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int _inputVal;
            if (value == null || !(int.TryParse(value.ToString(), out _inputVal)))
            {
                return value;
            }
            TimeSpan t = TimeSpan.FromSeconds(_inputVal);
            string tVal = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", 
                t.Hours, 
                t.Minutes, 
                t.Seconds);

            return (tVal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Hidden;

            var currentState = value.ToString();
            var stateStrings = parameter.ToString();
            var found = false;

            foreach (var state in stateStrings.Split(','))
            {
                found = (currentState == state.Trim());

                if (found)
                    break;
            }

            return found ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null || !(value is Enum))
                return Visibility.Hidden;

            var currentState = value.ToString();
            var stateStrings = parameter.ToString();
            var found = false;

            foreach (var state in stateStrings.Split(','))
            {
                found = (currentState == state.Trim());

                if (found)
                    break;
            }

            return found ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextTruncateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            if (parameter == null)
                return value;
            int _MaxLength;
            if (!int.TryParse(parameter.ToString(), out _MaxLength))
                return value;
            var _String = value.ToString().Replace("\r\n", "... ").Replace("\n", "... ").Replace("\r", "... ");
            if (_String.Length > _MaxLength)
                _String = _String.Substring(0, _MaxLength) + "...";
            return _String;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringLengthToMarginConverter : IValueConverter
    {

        public int LeftMargin { get; set; }
        public int OtherMargin { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int lvl = 0;
            if (value==null)
            {

            }
            else
            {
                lvl = ((string)value).Length;
            }
            return new Thickness(6 * lvl, 2, 2, 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
