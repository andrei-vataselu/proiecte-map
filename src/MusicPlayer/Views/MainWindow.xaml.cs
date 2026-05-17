using System.Windows;
using System.Windows.Input;
using MusicPlayer.ViewModels;

namespace MusicPlayer.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Closed += OnClosed;
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
            viewModel.Dispose();
    }

    private void OnSeekStarted(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
            viewModel.BeginSeek();
    }

    private void OnSeekEnded(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
            viewModel.EndSeek();
    }
}
