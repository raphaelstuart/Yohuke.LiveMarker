using System;
using System.Collections.ObjectModel;

namespace Yohuke.LiveMarker.Models;

public class LiveMarkerData
{
    public DateTime StartTime { get; set; }
    public ObservableCollection<MarkerData> Markers { get; set; } = new();
}