using System.Xml;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Adapter;

public sealed class LegacyXmlParser
{
    public LegacyDocument ParseXml(XmlDocument xml)
    {
        var header = xml.SelectSingleNode("/legacyDocument/header")?.InnerText ?? string.Empty;
        var body = xml.SelectSingleNode("/legacyDocument/body")?.InnerText ?? string.Empty;
        return new LegacyDocument(header, body);
    }
}
