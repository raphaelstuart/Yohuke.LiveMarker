using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Actions;

public class EditMarkerAction : IUndoableAction
{
    private readonly MarkerData marker;
    private readonly MarkerData oldSnapshot;
    private readonly MarkerData newSnapshot;

    public string Description => "Edit Marker";

    public EditMarkerAction(MarkerData marker, MarkerData oldSnapshot, MarkerData newSnapshot)
    {
        this.marker = marker;
        this.oldSnapshot = oldSnapshot;
        this.newSnapshot = newSnapshot;
    }

    public void Execute()
    {
        ApplySnapshot(newSnapshot);
    }

    public void Undo()
    {
        ApplySnapshot(oldSnapshot);
    }

    private void ApplySnapshot(MarkerData snapshot)
    {
        marker.Message = snapshot.Message;
        marker.MarkerColor = snapshot.MarkerColor;
        marker.RealDateTime = snapshot.RealDateTime;
        marker.LiveTime = snapshot.LiveTime;
    }
}