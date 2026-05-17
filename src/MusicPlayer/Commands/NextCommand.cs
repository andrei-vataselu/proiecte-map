using MusicPlayer.Controllers;

namespace MusicPlayer.Commands;

public sealed class NextCommand : IPlayerCommand
{
    private readonly PlaybackController _controller;

    public NextCommand(PlaybackController controller) => _controller = controller;

    public bool CanUndo => false;

    public string Description => "Următorul";

    public void Execute() => _controller.PlayNext(manualSkip: true);

    public void Undo()
    {
    }
}
