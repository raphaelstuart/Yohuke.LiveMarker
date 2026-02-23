using System;
using Avalonia.Media;

namespace Yohuke.LiveMarker.Models;

public readonly struct MarkerColorDefinition(MarkerColor color, IBrush displayColor) : IEquatable<MarkerColorDefinition>
{
    public MarkerColor Color { get; } = color;
    public IBrush DisplayColor { get; } = displayColor;
    
    public string Name => AppRuntime.I18N.GetText($"Color_{Color}");

    public bool Equals(MarkerColorDefinition other)
    {
        return Color == other.Color;
    }

    public override bool Equals(object obj)
    {
        return obj is MarkerColorDefinition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Color);
    }
}