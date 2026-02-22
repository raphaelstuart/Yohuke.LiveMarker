using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Converters;

public class MarkerColorToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MarkerColor color)
        {
            return color.ToColor().DisplayColor;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IBrush brush)
        {
            foreach (var colorDef in MarkerColorUtilities.ColorChoices)
            {
                if (Equals(colorDef.DisplayColor, brush))
                {
                    return colorDef.Color;
                }
            }
        }
        
        return AvaloniaProperty.UnsetValue;
    }
}