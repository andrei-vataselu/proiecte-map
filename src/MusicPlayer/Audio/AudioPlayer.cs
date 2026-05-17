using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using MusicPlayer.Models;
using NAudio.Wave;

namespace MusicPlayer.Audio;

public sealed class AudioPlayer : INotifyPropertyChanged, IDisposable
{
    private readonly DispatcherTimer _positionTimer;
    private IWavePlayer? _waveOut;
    private AudioFileReader? _reader;
    private Track? _currentTrack;
    private PlayerState _state = PlayerState.Stopped;
    private TimeSpan _position;
    private TimeSpan _duration;
    private double _volume = 0.8;
    private bool _manualStop;
    private bool _disposed;

    public AudioPlayer()
    {
        _positionTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(200)
        };
        _positionTimer.Tick += (_, _) => RefreshPosition();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? TrackEnded;

    public Track? CurrentTrack
    {
        get => _currentTrack;
        private set => SetField(ref _currentTrack, value);
    }

    public PlayerState State
    {
        get => _state;
        private set => SetField(ref _state, value);
    }

    public TimeSpan Position
    {
        get => _position;
        private set => SetField(ref _position, value);
    }

    public TimeSpan Duration
    {
        get => _duration;
        private set => SetField(ref _duration, value);
    }

    public double Volume
    {
        get => _volume;
        set
        {
            var clamped = Math.Clamp(value, 0.0, 1.0);
            if (SetField(ref _volume, clamped) && _reader is not null)
                _reader.Volume = (float)clamped;
        }
    }

    public void Load(Track track)
    {
        StopInternal(raiseEnded: false);
        DisposeReader();

        _currentTrack = track;
        _reader = new AudioFileReader(track.FilePath) { Volume = (float)_volume };
        _duration = _reader.TotalTime;
        Position = TimeSpan.Zero;

        _waveOut = new WaveOutEvent();
        _waveOut.PlaybackStopped += OnPlaybackStopped;
        _waveOut.Init(_reader);

        OnPropertyChanged(nameof(CurrentTrack));
        OnPropertyChanged(nameof(Duration));
    }

    public void Play()
    {
        if (_waveOut is null || _reader is null)
            return;

        _manualStop = false;
        _waveOut.Play();
        State = PlayerState.Playing;
        _positionTimer.Start();
    }

    public void Pause()
    {
        if (_waveOut is null)
            return;

        _waveOut.Pause();
        State = PlayerState.Paused;
        _positionTimer.Stop();
        RefreshPosition();
    }

    public void Stop()
    {
        _manualStop = true;
        StopInternal(raiseEnded: false);
    }

    public void Seek(TimeSpan position)
    {
        if (_reader is null)
            return;

        var target = position < TimeSpan.Zero
            ? TimeSpan.Zero
            : position > Duration ? Duration : position;

        _reader.CurrentTime = target;
        Position = target;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _positionTimer.Stop();
        StopInternal(raiseEnded: false);
        DisposeReader();
        _waveOut?.Dispose();
        _waveOut = null;
    }

    private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
    {
        _positionTimer.Stop();
        RefreshPosition();

        if (_manualStop || _reader is null)
        {
            State = PlayerState.Stopped;
            return;
        }

        var naturalEnd = Position >= Duration - TimeSpan.FromMilliseconds(300);
        State = PlayerState.Stopped;

        if (naturalEnd)
            TrackEnded?.Invoke(this, EventArgs.Empty);
    }

    private void StopInternal(bool raiseEnded)
    {
        _positionTimer.Stop();

        if (_waveOut is not null)
        {
            _waveOut.Stop();
            _waveOut.PlaybackStopped -= OnPlaybackStopped;
        }

        if (_reader is not null)
            Position = _reader.CurrentTime;

        State = PlayerState.Stopped;

        if (!raiseEnded)
            return;
    }

    private void RefreshPosition()
    {
        if (_reader is null)
            return;

        Position = _reader.CurrentTime;
    }

    private void DisposeReader()
    {
        _reader?.Dispose();
        _reader = null;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(name);
        return true;
    }
}
