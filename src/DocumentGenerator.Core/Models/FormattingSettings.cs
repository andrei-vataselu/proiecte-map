namespace DocumentGenerator.Core.Models;

public class FormattingSettings
{
    public PageFormat PageFormat { get; set; } = PageFormat.A4;
    public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;
    public bool IncludePageNumbers { get; set; } = true;

    public FormattingSettings Clone() =>
        new()
        {
            PageFormat = PageFormat,
            Orientation = Orientation,
            IncludePageNumbers = IncludePageNumbers
        };
}
