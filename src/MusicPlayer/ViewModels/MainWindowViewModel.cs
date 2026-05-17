using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MusicPlayer.Audio;
using MusicPlayer.Commands;
using MusicPlayer.Controllers;
using MusicPlayer.Models;
using MusicPlayer.Observers;
using MusicPlayer.Strategies;

namespace MusicPlayer.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly AudioPlayer _player;
    private readonly Playlist _playlist;
    private readonly PlaybackController _controller;
    private readonly CommandHistory _history;
    private readonly PlaybackLogger _logger;
    private readonly StatisticsTracker _statistics;
    private readonly AutoNextHandler _autoNext;
    private readonly ObservableCollection<TrackRowViewModel> _trackRows = new();
    private bool _isSeeking;
    private double _volumePercent = 80;

    public MainWindowViewModel()
    {
        _playlist = new Playlist();
        _player = new AudioPlayer { Volume = 0.8 };
        _controller = new PlaybackController(_player, _playlist, StrategyCatalog.CreateDefault());
        _history = new CommandHistory();
        _logger = new PlaybackLogger(_player, _controller);
        _statistics = new StatisticsTracker(_player);
        _autoNext = new AutoNextHandler(_player, _controller);

        Strategies = new ObservableCollection<StrategyOptionViewModel>(
            StrategyCatalog.Templates.Select(s => new StrategyOptionViewModel(s)));

        SyncStrategySelection();

        _playlist.TracksChanged += (_, _) => RebuildTrackRows();
        _player.PropertyChanged += OnPlayerPropertyChanged;
        _controller.StrategyChanged += (_, _) => SyncStrategySelection();
        _history.HistoryChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(HistoryEntries));
            RaiseCommands();
        };
        _history.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(CommandHistory.CanUndo) or nameof(CommandHistory.CanRedo))
                RaiseCommands();
        };

        _logger.Attach();
        _statistics.Attach();
        _statistics.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(StatisticsTracker.Snapshot))
                UpdateTransport();
        };
        _autoNext.Attach();

        RebuildTrackRows();
        UpdateNowPlaying();
        UpdateTransport();

        AddFilesCommand = new RelayCommand(AddFiles);
        UndoCommand = new RelayCommand(() => _history.Undo(), () => _history.CanUndo);
        RedoCommand = new RelayCommand(() => _history.Redo(), () => _history.CanRedo);
        PlayPauseCommand = new RelayCommand(TogglePlayPause, CanTogglePlayPause);
        NextCommand = new RelayCommand(ExecuteNext, () => _controller.CanGoNext());
        PreviousCommand = new RelayCommand(ExecutePrevious, () => _controller.CanGoPrevious());
        SelectStrategyCommand = new RelayCommand(SelectStrategy);
        UndoHistoryEntryCommand = new RelayCommand(UndoHistoryEntry, CanUndoHistoryEntry);
        PlayTrackCommand = new RelayCommand(PlayTrack, o => o is TrackRowViewModel);
        ClearPlaylistCommand = new RelayCommand(
            () => _history.Execute(new ClearPlaylistCommand(_playlist)),
            () => _playlist.Count > 0);
    }

    public ObservableCollection<TrackRowViewModel> TrackRows => _trackRows;

    public ObservableCollection<StrategyOptionViewModel> Strategies { get; }

    public ReadOnlyObservableCollection<HistoryEntry> HistoryEntries => _history.RecentEntries;

    public string LibraryHeader => $"BIBLIOTECĂ · {_playlist.Count} piese";

    public string HistoryHeader => $"ISTORIC · ultimele {Math.Min(10, _history.RecentEntries.Count)}";

    public string NowPlayingTitle => _player.CurrentTrack?.Title ?? "Nicio piesă";

    public string NowPlayingArtist => _player.CurrentTrack?.Artist ?? "—";

    public string NowPlayingAlbum => _player.CurrentTrack?.AlbumLine ?? "—";

    public string PositionText => FormatTime(_player.Position);

    public string DurationText => FormatTime(_player.Duration);

    public double SliderPosition
    {
        get => _player.Position.TotalSeconds;
        set
        {
            if (_isSeeking)
                _player.Seek(TimeSpan.FromSeconds(value));
        }
    }

    public double DurationSeconds => Math.Max(1, _player.Duration.TotalSeconds);

    public double VolumePercent
    {
        get => _volumePercent;
        set
        {
            _volumePercent = value;
            _player.Volume = value / 100.0;
            OnPropertyChanged();
        }
    }

    public bool IsPlaying => _player.State == PlayerState.Playing;

    public string PlayPauseGlyph => IsPlaying ? "⏸" : "▶";

    public string StatisticsTotal => _statistics.Snapshot.TotalPlayedText;

    public string StatisticsTopArtist => _statistics.Snapshot.TopArtist;

    public string StatisticsSkips => _statistics.Snapshot.Skips.ToString();

    public ICommand AddFilesCommand { get; }

    public ICommand UndoCommand { get; }

    public ICommand RedoCommand { get; }

    public ICommand PlayPauseCommand { get; }

    public ICommand NextCommand { get; }

    public ICommand PreviousCommand { get; }

    public ICommand SelectStrategyCommand { get; }

    public ICommand UndoHistoryEntryCommand { get; }

    public ICommand PlayTrackCommand { get; }

    public ICommand ClearPlaylistCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void BeginSeek() => _isSeeking = true;

    public void EndSeek() => _isSeeking = false;

    public void Dispose()
    {
        _playlist.TracksChanged -= (_, _) => RebuildTrackRows();
        _player.PropertyChanged -= OnPlayerPropertyChanged;
        _logger.Dispose();
        _statistics.Dispose();
        _autoNext.Dispose();
        _player.Dispose();
    }

    private void AddFiles()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Fișiere audio|*.mp3;*.wav",
            Multiselect = true,
            Title = "Adaugă fișiere audio"
        };

        if (dialog.ShowDialog() != true)
            return;

        foreach (var path in dialog.FileNames)
        {
            var track = Mp3MetadataReader.ReadFromFile(path);
            _history.Execute(new AddTrackCommand(_playlist, track));
        }

        if (_player.CurrentTrack is null && _playlist.Count > 0)
        {
            var first = _playlist.GetAt(0)!;
            _controller.LoadAndPlay(first);
        }
    }

    private void TogglePlayPause()
    {
        if (_player.State == PlayerState.Playing)
        {
            _history.Execute(new PauseCommand(_player));
            return;
        }

        if (_player.CurrentTrack is null)
        {
            var first = _playlist.GetAt(0);
            if (first is null)
                return;

            _controller.LoadAndPlay(first);
            return;
        }

        _history.Execute(new PlayCommand(_player));
    }

    private bool CanTogglePlayPause() =>
        _player.CurrentTrack is not null || _playlist.Count > 0;

    private void ExecuteNext()
    {
        RegisterSkipIfNeeded();
        _history.Execute(new Commands.NextCommand(_controller));
    }

    private void ExecutePrevious()
    {
        RegisterSkipIfNeeded();
        _history.Execute(new Commands.PreviousCommand(_controller));
    }

    private void RegisterSkipIfNeeded()
    {
        if (_player.State == PlayerState.Playing && _player.Position < TimeSpan.FromSeconds(30))
            _statistics.RegisterSkip();
    }

    private void SelectStrategy(object? parameter)
    {
        if (parameter is not StrategyOptionViewModel option)
            return;

        if (StrategyCatalog.SameKind(option.Strategy, _controller.CurrentStrategy))
            return;

        var fresh = StrategyCatalog.CreateNew(option.Strategy);
        _history.Execute(new ChangeStrategyCommand(_controller, fresh));
    }

    private void UndoHistoryEntry(object? parameter)
    {
        if (parameter is not HistoryEntry entry || !entry.IsLatest)
            return;

        _history.Undo();
    }

    private bool CanUndoHistoryEntry(object? parameter) =>
        parameter is HistoryEntry entry && entry.IsLatest && _history.CanUndo;

    private void PlayTrack(object? parameter)
    {
        if (parameter is not TrackRowViewModel row)
            return;

        _controller.LoadAndPlay(row.Track);
    }

    private void OnPlayerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AudioPlayer.Duration)
            or nameof(AudioPlayer.State) or nameof(AudioPlayer.CurrentTrack))
        {
            UpdateTransport();
            UpdateNowPlaying();
            UpdateTrackHighlights();
        }

        if (e.PropertyName == nameof(AudioPlayer.Position))
        {
            if (!_isSeeking)
                OnPropertyChanged(nameof(SliderPosition));

            OnPropertyChanged(nameof(PositionText));
        }
    }

    private void RebuildTrackRows()
    {
        _trackRows.Clear();
        for (var i = 0; i < _playlist.Count; i++)
        {
            var track = _playlist.GetAt(i)!;
            _trackRows.Add(new TrackRowViewModel(i + 1, track));
        }

        UpdateTrackHighlights();
        OnPropertyChanged(nameof(LibraryHeader));
        OnPropertyChanged(nameof(HistoryHeader));
        RaiseCommands();
    }

    private void UpdateTrackHighlights()
    {
        var current = _player.CurrentTrack;
        for (var i = 0; i < _trackRows.Count; i++)
        {
            var row = _trackRows[i];
            row.SetIndex(i + 1);
            var isCurrent = current is not null && row.Track.Id == current.Id;
            row.IsCurrent = isCurrent;
            row.Prefix = isCurrent
                ? (IsPlaying ? "▶" : "⏸")
                : row.Index.ToString();
        }
    }

    private void UpdateNowPlaying()
    {
        OnPropertyChanged(nameof(NowPlayingTitle));
        OnPropertyChanged(nameof(NowPlayingArtist));
        OnPropertyChanged(nameof(NowPlayingAlbum));
    }

    private void UpdateTransport()
    {
        OnPropertyChanged(nameof(PositionText));
        OnPropertyChanged(nameof(DurationText));
        OnPropertyChanged(nameof(SliderPosition));
        OnPropertyChanged(nameof(DurationSeconds));
        OnPropertyChanged(nameof(IsPlaying));
        OnPropertyChanged(nameof(PlayPauseGlyph));
        OnPropertyChanged(nameof(StatisticsTotal));
        OnPropertyChanged(nameof(StatisticsTopArtist));
        OnPropertyChanged(nameof(StatisticsSkips));
        RaiseCommands();
    }

    private void SyncStrategySelection()
    {
        foreach (var option in Strategies)
            option.IsSelected = StrategyCatalog.SameKind(option.Strategy, _controller.CurrentStrategy);
    }

    private void RaiseCommands()
    {
        CommandManager.InvalidateRequerySuggested();
        (PlayPauseCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private static string FormatTime(TimeSpan time) =>
        time.TotalHours >= 1
            ? $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}"
            : $"{time.Minutes}:{time.Seconds:D2}";

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
