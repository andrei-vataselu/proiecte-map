using MusicPlayer.Models;
using MusicPlayer.Strategies;
using NUnit.Framework;

namespace MusicPlayer.Tests;

public class StrategyTests
{
    private static Playlist CreatePlaylist(int count)
    {
        var playlist = new Playlist();
        for (var i = 0; i < count; i++)
        {
            playlist.Add(new Track(
                Guid.NewGuid(),
                $"Piesa {i + 1}",
                "Artist",
                "Album",
                TimeSpan.FromMinutes(3),
                $"c:\\track{i}.mp3"));
        }

        return playlist;
    }

    [Test]
    public void SequentialStrategy_MergesInOrder()
    {
        var playlist = CreatePlaylist(3);
        var strategy = new SequentialStrategy();
        var first = playlist.GetAt(0)!;
        var second = strategy.GetNextTrack(playlist, first);
        var third = strategy.GetNextTrack(playlist, second);
        var end = strategy.GetNextTrack(playlist, third);

        Assert.That(second, Is.SameAs(playlist.GetAt(1)));
        Assert.That(third, Is.SameAs(playlist.GetAt(2)));
        Assert.That(end, Is.Null);
    }

    [Test]
    public void ShuffleStrategy_CoversAllTracksBeforeRepeat()
    {
        var playlist = CreatePlaylist(4);
        var strategy = new ShuffleStrategy();
        strategy.Reset(playlist);

        var seen = new HashSet<Guid>();
        var current = strategy.GetNextTrack(playlist, null)!;
        seen.Add(current.Id);

        for (var i = 0; i < 3; i++)
        {
            current = strategy.GetNextTrack(playlist, current)!;
            Assert.That(seen.Add(current.Id), Is.True);
        }
    }

    [Test]
    public void SmartShuffleStrategy_AvoidsRecentTracks()
    {
        var playlist = CreatePlaylist(6);
        var strategy = new SmartShuffleStrategy();
        strategy.Reset(playlist);

        var current = playlist.GetAt(0)!;
        var recent = new List<Track> { current };

        for (var i = 0; i < 8; i++)
        {
            current = strategy.GetNextTrack(playlist, current)!;
            Assert.That(recent.TakeLast(5), Does.Not.Contain(current));
            recent.Add(current);
        }
    }

    [Test]
    public void RepeatOneStrategy_ReturnsSameTrack()
    {
        var playlist = CreatePlaylist(3);
        var strategy = new RepeatOneStrategy();
        var current = playlist.GetAt(1)!;

        Assert.That(strategy.GetNextTrack(playlist, current), Is.SameAs(current));
        Assert.That(strategy.GetPreviousTrack(playlist, current), Is.SameAs(current));
    }
}
