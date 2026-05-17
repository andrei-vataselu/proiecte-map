using MusicPlayer.Audio;
using MusicPlayer.Models;
using MusicPlayer.Strategies;

namespace MusicPlayer.Controllers;

public sealed class PlaybackController
{
    private IPlaybackStrategy _strategy;

    public PlaybackController(
        AudioPlayer player,
        Playlist playlist,
        IPlaybackStrategy initialStrategy)
    {
        Player = player;
        Playlist = playlist;
        _strategy = initialStrategy;
        _strategy.Reset(Playlist);
    }

    public AudioPlayer Player { get; }

    public Playlist Playlist { get; }

    public IPlaybackStrategy CurrentStrategy => _strategy;

    public event EventHandler<IPlaybackStrategy>? StrategyChanged;

    public void SetStrategy(IPlaybackStrategy strategy)
    {
        _strategy = strategy;
        _strategy.Reset(Playlist);
        StrategyChanged?.Invoke(this, strategy);
    }

    public void LoadAndPlay(Track track)
    {
        Player.Load(track);
        Player.Play();
    }

    public void PlayNext(bool manualSkip = false)
    {
        var next = _strategy.GetNextTrack(Playlist, Player.CurrentTrack);
        if (next is null)
        {
            Player.Stop();
            return;
        }

        LoadAndPlay(next);
    }

    public void PlayPrevious(bool manualSkip = false)
    {
        var previous = _strategy.GetPreviousTrack(Playlist, Player.CurrentTrack);
        if (previous is null)
            return;

        LoadAndPlay(previous);
    }

    public bool CanGoNext() => Playlist.Count > 0;

    public bool CanGoPrevious() =>
        Playlist.Count > 0 && Player.CurrentTrack is not null;
}
