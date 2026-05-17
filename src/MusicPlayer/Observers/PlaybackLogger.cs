using MusicPlayer.Audio;
using MusicPlayer.Controllers;
using MusicPlayer.Models;
using MusicPlayer.Strategies;

namespace MusicPlayer.Observers;

public sealed class PlaybackLogger : IDisposable
{
    private readonly AudioPlayer _player;
    private readonly PlaybackController _controller;
    private readonly string _logPath;
    private bool _attached;

    public PlaybackLogger(AudioPlayer player, PlaybackController controller, string? logPath = null)
    {
        _player = player;
        _controller = controller;
        _logPath = logPath ?? System.IO.Path.Combine(AppContext.BaseDirectory, "playback_log.txt");
    }

    public void Attach()
    {
        if (_attached)
            return;

        _player.PropertyChanged += OnPlayerChanged;
        _player.TrackEnded += OnTrackEnded;
        _controller.StrategyChanged += OnStrategyChanged;
        _attached = true;
    }

    public void Detach()
    {
        if (!_attached)
            return;

        _player.PropertyChanged -= OnPlayerChanged;
        _player.TrackEnded -= OnTrackEnded;
        _controller.StrategyChanged -= OnStrategyChanged;
        _attached = false;
    }

    public void Dispose() => Detach();

    private void OnPlayerChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AudioPlayer.CurrentTrack) && _player.CurrentTrack is Track track)
            Write($"TrackStarted: {track.DisplayTitle}");
    }

    private void OnTrackEnded(object? sender, EventArgs e) =>
        Write("TrackEnded: final natural");

    private void OnStrategyChanged(object? sender, IPlaybackStrategy strategy) =>
        Write($"StrategyChanged: {strategy.Name}");

    private void Write(string message)
    {
        var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
        System.IO.File.AppendAllText(_logPath, line + Environment.NewLine);
    }
}
