using Avalonia.Media;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Models;

public class MarkerColorDefinition(MarkerColor color, IBrush displayColor)
{
    public MarkerColor Color { get; } = color;
    public IBrush DisplayColor { get; } = displayColor;
}