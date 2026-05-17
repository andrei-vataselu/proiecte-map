using MusicPlayer.Models;

namespace MusicPlayer.Strategies;

public sealed class ShuffleStrategy : IPlaybackStrategy
{
    private readonly Random _random = new();
    private List<Track> _order = new();
    private int _index;

    public string Name => "Aleator";

    public void Reset(Playlist playlist) => BuildOrder(playlist);

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0)
            return null;

        if (_order.Count != playlist.Count)
            BuildOrder(playlist);

        if (currentTrack is null)
            return _order[0];

        var position = _order.IndexOf(currentTrack);
        if (position < 0)
        {
            _index = 0;
            return _order[0];
        }

        _index = position + 1;
        if (_index >= _order.Count)
        {
            BuildOrder(playlist);
            _index = 0;
        }

        return _order[_index];
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0 || currentTrack is null)
            return null;

        if (_order.Count != playlist.Count)
            BuildOrder(playlist);

        var position = _order.IndexOf(currentTrack);
        if (position <= 0)
            return _order[0];

        return _order[position - 1];
    }

    private void BuildOrder(Playlist playlist)
    {
        _order = playlist.Snapshot().OrderBy(_ => _random.Next()).ToList();
        _index = 0;
    }
}
