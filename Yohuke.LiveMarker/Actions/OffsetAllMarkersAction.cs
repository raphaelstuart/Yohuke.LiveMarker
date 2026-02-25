using System;
using System.Collections.Generic;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Actions;

public class OffsetAllMarkersAction : IUndoableAction
{
    private readonly IList<MarkerData> markers;
    private readonly TimeSpan offset;

    public string Description => "Offset All Markers";

    public OffsetAllMarkersAction(IList<MarkerData> markers, TimeSpan offset)
    {
        this.markers = markers;
        this.offset = offset;
    }

    public void Execute()
    {
        foreach (var marker in markers)
        {
            marker.LiveTime += offset;
        }
    }

    public void Undo()
    {
        foreach (var marker in markers)
        {
            marker.LiveTime -= offset;
        }
    }
}

