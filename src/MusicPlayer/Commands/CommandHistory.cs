using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicPlayer.Commands;

public sealed class CommandHistory : INotifyPropertyChanged
{
    private const int MaxHistory = 50;
    private const int MaxDisplayed = 10;
    private readonly Stack<IPlayerCommand> _undoStack = new();
    private readonly Stack<IPlayerCommand> _redoStack = new();
    private readonly ObservableCollection<HistoryEntry> _recentEntries = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? HistoryChanged;

    public ReadOnlyObservableCollection<HistoryEntry> RecentEntries { get; }

    public bool CanUndo => _undoStack.Count > 0;

    public bool CanRedo => _redoStack.Count > 0;

    public CommandHistory()
    {
        RecentEntries = new ReadOnlyObservableCollection<HistoryEntry>(_recentEntries);
    }

    public void Execute(IPlayerCommand command)
    {
        command.Execute();
        _redoStack.Clear();

        if (!command.CanUndo)
        {
            Notify();
            return;
        }

        _undoStack.Push(command);
        TrimUndoStack();
        PrependEntry(command);
        Notify();
    }

    public void Undo()
    {
        if (_undoStack.Count == 0)
            return;

        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);

        if (_recentEntries.Count > 0)
            _recentEntries.RemoveAt(0);

        MarkLatest();
        Notify();
    }

    public void Redo()
    {
        if (_redoStack.Count == 0)
            return;

        var command = _redoStack.Pop();
        command.Execute();
        _undoStack.Push(command);
        TrimUndoStack();
        PrependEntry(command);
        Notify();
    }

    private void TrimUndoStack()
    {
        if (_undoStack.Count <= MaxHistory)
            return;

        var items = _undoStack.Reverse().TakeLast(MaxHistory).ToList();
        _undoStack.Clear();
        foreach (var item in items)
            _undoStack.Push(item);

        _recentEntries.Clear();
        foreach (var command in _undoStack.Reverse().Take(MaxDisplayed))
            _recentEntries.Add(HistoryEntry.FromCommand(command));

        MarkLatest();
    }

    private void PrependEntry(IPlayerCommand command)
    {
        _recentEntries.Insert(0, HistoryEntry.FromCommand(command));

        while (_recentEntries.Count > MaxDisplayed)
            _recentEntries.RemoveAt(_recentEntries.Count - 1);

        MarkLatest();
    }

    private void MarkLatest()
    {
        for (var i = 0; i < _recentEntries.Count; i++)
            _recentEntries[i].IsLatest = i == 0;
    }

    private void Notify()
    {
        OnPropertyChanged(nameof(CanUndo));
        OnPropertyChanged(nameof(CanRedo));
        HistoryChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public sealed class HistoryEntry : INotifyPropertyChanged
{
    private bool _isLatest;

    public HistoryEntry(string action, string detail, IPlayerCommand command)
    {
        Action = action;
        Detail = detail;
        Command = command;
    }

    public string Action { get; }

    public string Detail { get; }

    public IPlayerCommand Command { get; }

    public bool IsLatest
    {
        get => _isLatest;
        set
        {
            if (_isLatest == value)
                return;

            _isLatest = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLatest)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public static HistoryEntry FromCommand(IPlayerCommand command)
    {
        var parts = command.Description.Split(':', 2, StringSplitOptions.TrimEntries);
        if (parts.Length == 2)
            return new HistoryEntry(parts[0], parts[1], command);

        return new HistoryEntry(command.Description, string.Empty, command);
    }
}
