namespace MusicPlayer.Models;

public sealed record Track(
    Guid Id,
    string Title,
    string Artist,
    string Album,
    TimeSpan Duration,
    string FilePath,
    int? Year = null)
{
    public string DisplayTitle => string.IsNullOrWhiteSpace(Artist)
        ? Title
        : $"{Title} — {Artist}";

    public string AlbumLine => Year.HasValue
        ? $"{Album} · {Year.Value}"
        : Album;
}
