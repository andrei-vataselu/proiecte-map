using MusicPlayer.Models;

namespace MusicPlayer.Commands;

public sealed class ClearPlaylistCommand : IPlayerCommand
{
    private readonly Playlist _playlist;
    private List<Track>? _snapshot;

    public ClearPlaylistCommand(Playlist playlist) => _playlist = playlist;

    public bool CanUndo => true;

    public string Description => "Golește playlistul";

    public void Execute()
    {
        _snapshot = _playlist.Snapshot().ToList();
        _playlist.Clear();
    }

    public void Undo()
    {
        if (_snapshot is null)
            return;

        _playlist.Clear();
        foreach (var track in _snapshot)
            _playlist.Add(track);
    }
}
