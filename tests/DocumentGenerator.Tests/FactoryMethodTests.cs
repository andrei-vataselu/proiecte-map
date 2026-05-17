using DocumentGenerator.Core.Builder;
using DocumentGenerator.Core.Export;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class FactoryMethodTests
{
    [Test]
    public void HtmlAndPlainTextExporters_ProduceDifferentOutput_ForSameData()
    {
        var data = new DocumentDataBuilder()
            .WithTitle("Test Document")
            .ByAuthor("Tester")
            .WithSection("Sectiune", "Continut test")
            .Build();

        DocumentExporter htmlExporter = new HtmlDocumentExporter();
        DocumentExporter textExporter = new PlainTextDocumentExporter();

        var html = htmlExporter.Export(data);
        var text = textExporter.Export(data);

        Assert.That(html, Does.Contain("<html>"));
        Assert.That(text, Does.Not.Contain("<html>"));
        Assert.That(html, Is.Not.EqualTo(text));
    }
}
