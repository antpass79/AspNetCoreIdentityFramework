using Globe.Client.Platofrm.Events;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Globe.Client.Localizer.Converters
{
    public sealed class IsMergeableToRowBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMergeable = (bool)value;

            return isMergeable ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
