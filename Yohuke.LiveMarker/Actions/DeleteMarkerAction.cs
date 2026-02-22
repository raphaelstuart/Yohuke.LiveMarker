using System.Collections.ObjectModel;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Actions;

public class DeleteMarkerAction : IUndoableAction
{
    private readonly ObservableCollection<MarkerData> collection;
    private readonly MarkerData marker;
    private readonly int index;

    public string Description => "Delete Marker";

    public DeleteMarkerAction(ObservableCollection<MarkerData> collection, MarkerData marker)
    {
        this.collection = collection;
        this.marker = marker;
        index = collection.IndexOf(marker);
    }

    public void Execute()
    {
        collection.Remove(marker);
    }

    public void Undo()
    {
        if (index >= 0 && index <= collection.Count)
            collection.Insert(index, marker);
        else
            collection.Add(marker);
    }
}