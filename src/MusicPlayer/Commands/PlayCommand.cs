using MusicPlayer.Audio;

namespace MusicPlayer.Commands;

public sealed class PlayCommand : IPlayerCommand
{
    private readonly AudioPlayer _player;

    public PlayCommand(AudioPlayer player) => _player = player;

    public bool CanUndo => false;

    public string Description => "Redare";

    public void Execute() => _player.Play();

    public void Undo()
    {
    }
}
