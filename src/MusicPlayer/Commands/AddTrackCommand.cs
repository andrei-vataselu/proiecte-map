using MusicPlayer.Models;

namespace MusicPlayer.Commands;

public sealed class AddTrackCommand : IPlayerCommand
{
    private readonly Playlist _playlist;
    private readonly Track _track;

    public AddTrackCommand(Playlist playlist, Track track)
    {
        _playlist = playlist;
        _track = track;
    }

    public bool CanUndo => true;

    public string Description => $"Adaugă: \"{_track.Title}\"";

    public void Execute() => _playlist.Add(_track);

    public void Undo() => _playlist.Remove(_track);
}
