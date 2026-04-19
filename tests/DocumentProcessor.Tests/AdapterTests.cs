using DocumentProcessor.Core.Adapter;
using DocumentProcessor.Core.Parsers;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

public sealed class AdapterTests
{
    [Test]
    public void XmlAdapter_And_JsonParser_ProduceSameShape_ForEquivalentPayload()
    {
        const string title = "Hello";
        const string body = "1234567890";
        var xml =
            $"<legacyDocument><header>{title}</header><body>{body}</body></legacyDocument>";
        var json = $"{{\"title\":\"{title}\",\"content\":\"{body}\"}}";

        var xmlDoc = new XmlParserAdapter(new LegacyXmlParser()).Parse(xml);
        var jsonDoc = new JsonDocumentParser().Parse(json);

        Assert.That(xmlDoc.Title, Is.EqualTo(jsonDoc.Title));
        Assert.That(xmlDoc.Content, Is.EqualTo(jsonDoc.Content));
    }
}
