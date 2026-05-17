using MusicPlayer.Controllers;
using MusicPlayer.Strategies;

namespace MusicPlayer.Commands;

public sealed class ChangeStrategyCommand : IPlayerCommand
{
    private readonly PlaybackController _controller;
    private readonly IPlaybackStrategy _newStrategy;
    private IPlaybackStrategy? _previousStrategy;
    private string _description = string.Empty;

    public ChangeStrategyCommand(PlaybackController controller, IPlaybackStrategy newStrategy)
    {
        _controller = controller;
        _newStrategy = newStrategy;
    }

    public bool CanUndo => true;

    public string Description => _description;

    public void Execute()
    {
        _previousStrategy = _controller.CurrentStrategy;
        _description = $"Strategie: {_previousStrategy.Name} → {_newStrategy.Name}";
        _controller.SetStrategy(_newStrategy);
    }

    public void Undo()
    {
        if (_previousStrategy is not null)
            _controller.SetStrategy(_previousStrategy);
    }
}
