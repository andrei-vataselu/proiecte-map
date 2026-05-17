namespace MusicPlayer.Commands;

public interface IPlayerCommand
{
    void Execute();

    void Undo();

    bool CanUndo { get; }

    string Description { get; }
}
