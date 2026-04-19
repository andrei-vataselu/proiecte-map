using System.Xml;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Adapter;

public sealed class XmlParserAdapter : IDocumentParser
{
    private readonly LegacyXmlParser _legacy;

    public XmlParserAdapter(LegacyXmlParser legacy)
    {
        _legacy = legacy;
    }

    public Document Parse(string content)
    {
        var xml = new XmlDocument();
        xml.LoadXml(content);
        var legacy = _legacy.ParseXml(xml);
        return new Document(legacy.Header, legacy.Body, DocumentSourceFormat.XmlLegacy);
    }
}
