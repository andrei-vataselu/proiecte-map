using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicPlayer.Models;

public sealed class Playlist
{
    private readonly ObservableCollection<Track> _tracks = new();

    public Playlist()
    {
        Tracks = new ReadOnlyObservableCollection<Track>(_tracks);
        _tracks.CollectionChanged += (_, e) => TracksChanged?.Invoke(this, e);
    }

    public ReadOnlyObservableCollection<Track> Tracks { get; }

    public event NotifyCollectionChangedEventHandler? TracksChanged;

    public int Count => _tracks.Count;

    public Track? GetAt(int index) =>
        index >= 0 && index < _tracks.Count ? _tracks[index] : null;

    public int IndexOf(Track track) => _tracks.IndexOf(track);

    public void Add(Track track) => _tracks.Add(track);

    public void Insert(int index, Track track) => _tracks.Insert(index, track);

    public bool Remove(Track track) => _tracks.Remove(track);

    public void Move(int oldIndex, int newIndex) => _tracks.Move(oldIndex, newIndex);

    public void Clear() => _tracks.Clear();

    public IReadOnlyList<Track> Snapshot() => _tracks.ToList();
}
