using MusicPlayer.Models;

namespace MusicPlayer.Strategies;

public sealed class SequentialStrategy : IPlaybackStrategy
{
    public string Name => "Secvențial";

    public void Reset(Playlist playlist)
    {
    }

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0)
            return null;

        if (currentTrack is null)
            return playlist.GetAt(0);

        var index = playlist.IndexOf(currentTrack);
        if (index < 0)
            return playlist.GetAt(0);

        var next = index + 1;
        return next < playlist.Count ? playlist.GetAt(next) : null;
    }

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack)
    {
        if (playlist.Count == 0 || currentTrack is null)
            return null;

        var index = playlist.IndexOf(currentTrack);
        if (index <= 0)
            return null;

        return playlist.GetAt(index - 1);
    }
}
