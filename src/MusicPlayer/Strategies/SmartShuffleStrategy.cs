using MusicPlayer.Models;

namespace MusicPlayer.Strategies;

public sealed class SmartShuffleStrategy : IPlaybackStrategy
{
    private const int DefaultWindow = 5;
    private readonly Random _random = new();
    private readonly Queue<Track> _history = new();

    public string Name => "Smart Shuffle";

    public void Reset(Playlist playlist) => _history.Clear();

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0)
            return null;

        if (currentTrack is not null)
            Remember(currentTrack, playlist);

        var candidates = playlist.Snapshot()
            .Where(t => !_history.Contains(t))
            .ToList();

        if (candidates.Count == 0)
        {
            _history.Clear();
            candidates = playlist.Snapshot().ToList();
        }

        var next = candidates[_random.Next(candidates.Count)];
        Remember(next, playlist);
        return next;
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0 || currentTrack is null)
            return null;

        var index = playlist.IndexOf(currentTrack);
        return index > 0 ? playlist.GetAt(index - 1) : null;
    }

    private void Remember(Track track, Playlist playlist)
    {
        if (_history.Contains(track))
        {
            var list = _history.Where(t => t != track).ToList();
            _history.Clear();
            foreach (var item in list)
                _history.Enqueue(item);
        }

        _history.Enqueue(track);
        var window = Math.Min(DefaultWindow, Math.Max(1, playlist.Count));
        while (_history.Count > window)
            _history.Dequeue();
    }
}
