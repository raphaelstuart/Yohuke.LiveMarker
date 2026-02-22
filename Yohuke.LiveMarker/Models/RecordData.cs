using System;

namespace Yohuke.LiveMarker.Models;

public class MarkerData
{
    public DateTime RealDateTime { get; set; }
    public TimeSpan LiveTime { get; set; }
    public string Message { get; set; }
    public MarkerColorDefinition MarkerColor { get; set; }
}