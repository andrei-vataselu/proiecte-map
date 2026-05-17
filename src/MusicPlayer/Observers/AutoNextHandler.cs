using MusicPlayer.Audio;
using MusicPlayer.Controllers;

namespace MusicPlayer.Observers;

public sealed class AutoNextHandler : IDisposable
{
    private readonly AudioPlayer _player;
    private readonly PlaybackController _controller;
    private bool _attached;

    public AutoNextHandler(AudioPlayer player, PlaybackController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void Attach()
    {
        if (_attached)
            return;

        _player.TrackEnded += OnTrackEnded;
        _attached = true;
    }

    public void Detach()
    {
        if (!_attached)
            return;

        _player.TrackEnded -= OnTrackEnded;
        _attached = false;
    }

    public void Dispose() => Detach();

    private void OnTrackEnded(object? sender, EventArgs e) =>
        _controller.PlayNext();
}
