#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Yohuke.LiveMarker.Converters;

public sealed class LiveTimeMultiConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 2 || values[0] is not DateTime real || values[1] is not DateTime start)
        {
            return AvaloniaProperty.UnsetValue;
        }

        var delta = real - start;
        return delta.ToString(@"hh\:mm\:ss\.fff", culture);
    }
}