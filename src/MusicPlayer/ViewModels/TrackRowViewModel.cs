using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public sealed class TrackRowViewModel : INotifyPropertyChanged
{
    private bool _isCurrent;
    private string _prefix = string.Empty;

    public TrackRowViewModel(int index, Track track)
    {
        Index = index;
        Track = track;
        Title = track.DisplayTitle;
        DurationText = Format(track.Duration);
    }

    public int Index { get; private set; }

    public Track Track { get; }

    public string Title { get; }

    public string DurationText { get; }

    public bool IsCurrent
    {
        get => _isCurrent;
        set => SetField(ref _isCurrent, value);
    }

    public string Prefix
    {
        get => _prefix;
        set => SetField(ref _prefix, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetIndex(int index) => Index = index;

    public static string Format(TimeSpan duration) =>
        duration.TotalHours >= 1
            ? $"{(int)duration.TotalHours}:{duration.Minutes:D2}:{duration.Seconds:D2}"
            : $"{duration.Minutes}:{duration.Seconds:D2}";

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
