using System.Collections.ObjectModel;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Actions;

public class AddMarkerAction : IUndoableAction
{
    private readonly ObservableCollection<MarkerData> collection;
    private readonly MarkerData marker;

    public string Description => "Add Marker";

    public AddMarkerAction(ObservableCollection<MarkerData> collection, MarkerData marker)
    {
        this.collection = collection;
        this.marker = marker;
    }

    public void Execute()
    {
        collection.Add(marker);
    }

    public void Undo()
    {
        collection.Remove(marker);
    }
}