using MusicPlayer.Models;

namespace MusicPlayer.Commands;

public sealed class RemoveTrackCommand : IPlayerCommand
{
    private readonly Playlist _playlist;
    private readonly Track _track;
    private int _index = -1;

    public RemoveTrackCommand(Playlist playlist, Track track)
    {
        _playlist = playlist;
        _track = track;
    }

    public bool CanUndo => true;

    public string Description => $"Elimină: \"{_track.Title}\"";

    public void Execute()
    {
        _index = _playlist.IndexOf(_track);
        _playlist.Remove(_track);
    }

    public void Undo()
    {
        if (_index < 0)
            return;

        if (_index >= _playlist.Count)
            _playlist.Add(_track);
        else
            _playlist.Insert(_index, _track);
    }
}
