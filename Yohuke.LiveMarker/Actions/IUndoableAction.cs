namespace Yohuke.LiveMarker.Actions;

public interface IUndoableAction
{
    string Description { get; }
    void Execute();
    void Undo();
}