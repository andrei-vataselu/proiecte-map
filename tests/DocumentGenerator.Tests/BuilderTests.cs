using DocumentGenerator.Core.Builder;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class BuilderTests
{
    [Test]
    public void Build_Throws_WhenTitleMissing()
    {
        var builder = new DocumentDataBuilder()
            .ByAuthor("Autor")
            .WithSection("Sectiune", "Continut");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Test]
    public void Build_Throws_WhenNoSections()
    {
        var builder = new DocumentDataBuilder()
            .WithTitle("Titlu")
            .ByAuthor("Autor");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
}
