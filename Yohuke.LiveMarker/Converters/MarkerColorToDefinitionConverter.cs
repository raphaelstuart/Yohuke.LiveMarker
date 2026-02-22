using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Converters;

public class MarkerColorToDefinitionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MarkerColor color)
        {
            return color.ToColor();
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MarkerColorDefinition def)
        {
            return def.Color;
        }
        
        return AvaloniaProperty.UnsetValue;
    }
}