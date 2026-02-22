using System.Collections.ObjectModel;
using Avalonia.Media;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Utilities;

public static class MarkerColorUtilities
{
    private static readonly ObservableCollection<MarkerColorDefinition> _colorChoices = new()
    {
        new (MarkerColor.Red, Brushes.Tomato),
        new (MarkerColor.Orange, Brushes.Orange),
        new (MarkerColor.Yellow, Brushes.Yellow),
        new (MarkerColor.Green, Brushes.LawnGreen),
        new (MarkerColor.Blue, Brushes.DodgerBlue),
        new (MarkerColor.Magenta, Brushes.Violet),
        new (MarkerColor.Grey, Brushes.Gray)
    };

    public static ObservableCollection<MarkerColorDefinition> ColorChoices => _colorChoices;
    public static MarkerColorDefinition DefaultColor => _colorChoices[0];
    public static MarkerColorDefinition Red => _colorChoices[0];
    public static MarkerColorDefinition Orange => _colorChoices[1];
    public static MarkerColorDefinition Yellow => _colorChoices[2];
    public static MarkerColorDefinition Green => _colorChoices[3];
    public static MarkerColorDefinition Blue => _colorChoices[4];
    public static MarkerColorDefinition Magenta => _colorChoices[5];
    public static MarkerColorDefinition Grey => _colorChoices[6];
    
    public static MarkerColorDefinition ToColor(this MarkerColor color)
    {
        return color switch
        {
            MarkerColor.Red => Red,
            MarkerColor.Orange => Orange,
            MarkerColor.Yellow => Yellow,
            MarkerColor.Green => Green,
            MarkerColor.Blue => Blue,
            MarkerColor.Magenta => Magenta,
            MarkerColor.Grey => Grey,
            _ => DefaultColor
        };
    }
}