using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicPlayer.Audio;
using MusicPlayer.Models;

namespace MusicPlayer.Observers;

public sealed class StatisticsSnapshot
{
    public StatisticsSnapshot(TimeSpan totalPlayed, string topArtist, int skips)
    {
        TotalPlayed = totalPlayed;
        TopArtist = topArtist;
        Skips = skips;
    }

    public TimeSpan TotalPlayed { get; }

    public string TopArtist { get; }

    public int Skips { get; }

    public string TotalPlayedText
    {
        get
        {
            if (TotalPlayed.TotalHours >= 1)
                return $"{(int)TotalPlayed.TotalHours}h {TotalPlayed.Minutes}m";

            return $"{TotalPlayed.Minutes}m {TotalPlayed.Seconds}s";
        }
    }
}

public sealed class StatisticsTracker : INotifyPropertyChanged, IDisposable
{
    private readonly AudioPlayer _player;
    private readonly Dictionary<string, TimeSpan> _artistMinutes = new();
    private readonly Dictionary<Guid, int> _playCounts = new();
    private DateTime _trackStartedAt;
    private Track? _listeningTrack;
    private bool _attached;

    public StatisticsTracker(AudioPlayer player) => _player = player;

    public StatisticsSnapshot Snapshot { get; private set; } =
        new(TimeSpan.Zero, "—", 0);

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Attach()
    {
        if (_attached)
            return;

        _player.PropertyChanged += OnPlayerChanged;
        _attached = true;
    }

    public void Detach()
    {
        if (!_attached)
            return;

        _player.PropertyChanged -= OnPlayerChanged;
        _attached = false;
    }

    public void RegisterSkip()
    {
        Snapshot = BuildSnapshot(Snapshot.Skips + 1);
        OnPropertyChanged(nameof(Snapshot));
    }

    public void Dispose() => Detach();

    private void OnPlayerChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AudioPlayer.CurrentTrack))
            HandleTrackChange();

        if (e.PropertyName == nameof(AudioPlayer.State) && _player.State == PlayerState.Playing)
            _trackStartedAt = DateTime.UtcNow;
    }

    private void HandleTrackChange()
    {
        if (_listeningTrack is not null)
            RegisterPlay(_listeningTrack, DateTime.UtcNow - _trackStartedAt);

        _listeningTrack = _player.CurrentTrack;
        _trackStartedAt = DateTime.UtcNow;

        if (_listeningTrack is not null)
        {
            _playCounts.TryGetValue(_listeningTrack.Id, out var count);
            _playCounts[_listeningTrack.Id] = count + 1;
        }

        Snapshot = BuildSnapshot(Snapshot.Skips);
        OnPropertyChanged(nameof(Snapshot));
    }

    private void RegisterPlay(Track track, TimeSpan listened)
    {
        if (listened <= TimeSpan.Zero)
            return;

        if (!_artistMinutes.ContainsKey(track.Artist))
            _artistMinutes[track.Artist] = TimeSpan.Zero;

        _artistMinutes[track.Artist] += listened;
        Snapshot = BuildSnapshot(Snapshot.Skips);
        OnPropertyChanged(nameof(Snapshot));
    }

    private StatisticsSnapshot BuildSnapshot(int skips)
    {
        var total = TimeSpan.FromTicks(_artistMinutes.Values.Sum(t => t.Ticks));
        var topArtist = _artistMinutes.Count == 0
            ? "—"
            : _artistMinutes.OrderByDescending(p => p.Value).First().Key;

        return new StatisticsSnapshot(total, topArtist, skips);
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
