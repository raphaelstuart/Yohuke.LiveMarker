using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Yohuke.LiveMarker.Converters;

public class LiveTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts)
        {
            return ts.ToString(@"hh\:mm\:ss");
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s && TimeSpan.TryParse(s, out var result))
        {
            return result;
        }
        throw new DataValidationException("Format is hh:mm:ss");
    }
}