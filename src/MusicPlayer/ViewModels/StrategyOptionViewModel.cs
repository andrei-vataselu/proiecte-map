using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicPlayer.Strategies;

namespace MusicPlayer.ViewModels;

public sealed class StrategyOptionViewModel : INotifyPropertyChanged
{
    private bool _isSelected;

    public StrategyOptionViewModel(IPlaybackStrategy strategy)
    {
        Strategy = strategy;
        Name = strategy.Name;
    }

    public IPlaybackStrategy Strategy { get; }

    public string Name { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
