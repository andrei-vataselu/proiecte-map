using MusicPlayer.Models;

namespace MusicPlayer.Strategies;

public sealed class RepeatOneStrategy : IPlaybackStrategy
{
    public string Name => "Repetă unul";

    public void Reset(Playlist playlist)
    {
    }

    public Track? GetNextTrack(Playlist playlist, Track? currentTrack) => currentTrack;

    public Track? GetPreviousTrack(Playlist playlist, Track? currentTrack) => currentTrack;
}
