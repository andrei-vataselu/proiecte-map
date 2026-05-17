using MusicPlayer.Models;
using NAudio.Wave;

namespace MusicPlayer.Audio;

public static class Mp3MetadataReader
{
    public static Track ReadFromFile(string path)
    {
        TimeSpan duration;
        var extension = System.IO.Path.GetExtension(path).ToLowerInvariant();

        if (extension == ".mp3")
        {
            using var reader = new Mp3FileReader(path);
            duration = reader.TotalTime;
        }
        else
        {
            using var reader = new AudioFileReader(path);
            duration = reader.TotalTime;
        }

        var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
        string title;
        string artist;

        var separator = fileName.IndexOf(" - ", StringComparison.Ordinal);
        if (separator > 0)
        {
            artist = fileName[..separator].Trim();
            title = fileName[(separator + 3)..].Trim();
        }
        else
        {
            title = fileName;
            artist = "Necunoscut";
        }

        return new Track(Guid.NewGuid(), title, artist, "Album necunoscut", duration, path);
    }
}
