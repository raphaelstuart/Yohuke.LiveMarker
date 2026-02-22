// Yohuke.LiveMarker/UndoRedo/UndoRedoManager.cs
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Yohuke.LiveMarker.Actions;

public partial class ActionManager : ObservableObject
{
    private readonly Stack<IUndoableAction> undoStack = new();
    private readonly Stack<IUndoableAction> redoStack = new();

    [ObservableProperty]
    private bool canUndo;

    [ObservableProperty]
    private bool canRedo;
    
    public void ExecuteAction(IUndoableAction action, bool executeNow = true)
    {
        if (executeNow)
            action.Execute();

        undoStack.Push(action);
        redoStack.Clear();
        UpdateState();
    }

    public void Undo()
    {
        if (undoStack.Count == 0) return;

        var action = undoStack.Pop();
        action.Undo();
        redoStack.Push(action);
        UpdateState();
    }

    public void Redo()
    {
        if (redoStack.Count == 0) return;

        var action = redoStack.Pop();
        action.Execute();
        undoStack.Push(action);
        UpdateState();
    }

    public void Clear()
    {
        undoStack.Clear();
        redoStack.Clear();
        UpdateState();
    }

    private void UpdateState()
    {CanUndo = undoStack.Count > 0;
        CanRedo = redoStack.Count > 0;
    }
}