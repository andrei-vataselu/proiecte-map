using MusicPlayer.Controllers;

namespace MusicPlayer.Commands;

public sealed class PreviousCommand : IPlayerCommand
{
    private readonly PlaybackController _controller;

    public PreviousCommand(PlaybackController controller) => _controller = controller;

    public bool CanUndo => false;

    public string Description => "Anteriorul";

    public void Execute() => _controller.PlayPrevious(manualSkip: true);

    public void Undo()
    {
    }
}
