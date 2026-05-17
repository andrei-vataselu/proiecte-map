using MusicPlayer.Commands;
using MusicPlayer.Models;
using NUnit.Framework;

namespace MusicPlayer.Tests;

public class CommandHistoryTests
{
    [Test]
    public void Execute_StoresOnlyUndoableCommands()
    {
        var history = new CommandHistory();
        var playlist = new Playlist();
        var track = new Track(Guid.NewGuid(), "A", "B", "C", TimeSpan.FromMinutes(1), "x.mp3");

        history.Execute(new AddTrackCommand(playlist, track));
        Assert.That(history.CanUndo, Is.True);
        Assert.That(playlist.Count, Is.EqualTo(1));
    }

    [Test]
    public void UndoRedo_RestoresPlaylistChange()
    {
        var history = new CommandHistory();
        var playlist = new Playlist();
        var track = new Track(Guid.NewGuid(), "A", "B", "C", TimeSpan.FromMinutes(1), "x.mp3");
        var command = new AddTrackCommand(playlist, track);

        history.Execute(command);
        history.Undo();
        Assert.That(playlist.Count, Is.EqualTo(0));

        history.Redo();
        Assert.That(playlist.Count, Is.EqualTo(1));
    }

    [Test]
    public void RemoveTrack_Undo_ReinsertsAtOriginalIndex()
    {
        var playlist = new Playlist();
        var tracks = Enumerable.Range(0, 3)
            .Select(i => new Track(Guid.NewGuid(), $"T{i}", "A", "B", TimeSpan.FromMinutes(1), $"{i}.mp3"))
            .ToList();

        foreach (var track in tracks)
            playlist.Add(track);

        var history = new CommandHistory();
        history.Execute(new RemoveTrackCommand(playlist, tracks[1]));
        Assert.That(playlist.GetAt(1)!.Title, Is.EqualTo("T2"));

        history.Undo();
        Assert.That(playlist.GetAt(1)!.Title, Is.EqualTo("T1"));
    }
}
