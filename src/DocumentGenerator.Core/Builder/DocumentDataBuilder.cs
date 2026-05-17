using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Builder;

public class DocumentDataBuilder
{
    private string? _title;
    private string? _author;
    private DateTime _date = DateTime.UtcNow;
    private readonly List<DocumentSection> _sections = new();
    private PageFormat _pageFormat = PageFormat.A4;
    private PageOrientation _orientation = PageOrientation.Portrait;
    private string? _footnote;

    public DocumentDataBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public DocumentDataBuilder ByAuthor(string author)
    {
        _author = author;
        return this;
    }

    public DocumentDataBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public DocumentDataBuilder WithSection(string heading, string content)
    {
        _sections.Add(new DocumentSection { Heading = heading, Content = content });
        return this;
    }

    public DocumentDataBuilder InLandscape()
    {
        _orientation = PageOrientation.Landscape;
        return this;
    }

    public DocumentDataBuilder WithFootnote(string footnote)
    {
        _footnote = footnote;
        return this;
    }

    public DocumentData Build()
    {
        if (string.IsNullOrWhiteSpace(_title))
            throw new InvalidOperationException("Titlul documentului este obligatoriu.");

        if (string.IsNullOrWhiteSpace(_author))
            throw new InvalidOperationException("Autorul documentului este obligatoriu.");

        if (_sections.Count == 0)
            throw new InvalidOperationException("Documentul trebuie sa contina cel putin o sectiune.");

        return new DocumentData
        {
            Title = _title.Trim(),
            Author = _author.Trim(),
            Date = _date,
            Sections = _sections.Select(s => s.Clone()).ToList(),
            PageFormat = _pageFormat,
            Orientation = _orientation,
            Footnote = _footnote
        };
    }
}
