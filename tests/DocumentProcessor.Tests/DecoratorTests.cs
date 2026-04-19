using DocumentProcessor.Core.Decorators;
using DocumentProcessor.Core.Exceptions;
using DocumentProcessor.Core.Models;
using DocumentProcessor.Tests.Helpers;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

public sealed class DecoratorTests
{
    [Test]
    public void Validation_ThrowsWhenTitleEmpty()
    {
        var inner = new ConfigurableFakeParser(_ =>
            new Document(string.Empty, "1234567890", DocumentSourceFormat.Json));
        var sut = new ValidationDocumentParser(inner);

        Assert.Throws<ValidationException>(() => sut.Parse("ignored"));
    }

    [Test]
    public void Caching_CallsInnerOnce_ForSameContent()
    {
        var inner = new CountingFakeParser();
        var sut = new CachingDocumentParser(inner);
        const string payload = "same-payload";

        var first = sut.Parse(payload);
        var second = sut.Parse(payload);

        Assert.That(inner.Calls, Is.EqualTo(1));
        Assert.That(first, Is.SameAs(second));
    }
}
