using DocumentGenerator.Core.Models;
using DocumentGenerator.Core.Prototype;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class PrototypeTests
{
    [Test]
    public void TwoClones_FromSamePrototype_AreIndependent()
    {
        var prototype = new DocumentTemplate
        {
            DefaultTitle = "Sablon",
            Sections = { new DocumentSection { Heading = "A", Content = "1" } }
        };

        var clone1 = prototype.DeepClone();
        var clone2 = prototype.DeepClone();

        clone1.Sections[0].Content = "modificat-1";
        clone2.Sections[0].Heading = "modificat-2";

        Assert.That(prototype.Sections[0].Content, Is.EqualTo("1"));
        Assert.That(prototype.Sections[0].Heading, Is.EqualTo("A"));
        Assert.That(clone1.Sections[0].Content, Is.EqualTo("modificat-1"));
        Assert.That(clone2.Sections[0].Heading, Is.EqualTo("modificat-2"));
    }

    [Test]
    public void ClonedDocument_FromRegistry_DoesNotModifyRegisteredPrototype()
    {
        var registry = new TemplateRegistry();
        registry.Register("test", new DocumentTemplate
        {
            DefaultTitle = "Original",
            Sections = { new DocumentSection { Heading = "H", Content = "C" } }
        });

        var clone = registry.GetClone("test");
        clone.DefaultTitle = "Modificat";
        clone.Sections[0].Content = "Alt continut";

        var secondClone = registry.GetClone("test");

        Assert.That(secondClone.DefaultTitle, Is.EqualTo("Original"));
        Assert.That(secondClone.Sections[0].Content, Is.EqualTo("C"));
    }
}
