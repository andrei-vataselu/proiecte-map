using MusicPlayer.Models;

namespace MusicPlayer.Strategies;

public interface IPlaybackStrategy
{
    string Name { get; }

    Track? GetNextTrack(Playlist playlist, Track? currentTrack);

    Track? GetPreviousTrack(Playlist playlist, Track? currentTrack);

    void Reset(Playlist playlist);
}
