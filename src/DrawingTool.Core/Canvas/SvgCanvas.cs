using System.Globalization;
using System.Text;
using DrawingTool.Core.Interfaces;

namespace DrawingTool.Core.Canvas;

public sealed class SvgCanvas : ICanvas
{
    private readonly List<string> _elements = new();

    public void DrawLine(double x1, double y1, double x2, double y2)
    {
        _elements.Add(
            $"<line x1=\"{Fmt(x1)}\" y1=\"{Fmt(y1)}\" x2=\"{Fmt(x2)}\" y2=\"{Fmt(y2)}\" stroke=\"black\" fill=\"none\" />");
    }

    public void DrawCircle(double cx, double cy, double r)
    {
        _elements.Add($"<circle cx=\"{Fmt(cx)}\" cy=\"{Fmt(cy)}\" r=\"{Fmt(r)}\" stroke=\"black\" fill=\"none\" />");
    }

    public void DrawRect(double x, double y, double w, double h)
    {
        _elements.Add(
            $"<rect x=\"{Fmt(x)}\" y=\"{Fmt(y)}\" width=\"{Fmt(w)}\" height=\"{Fmt(h)}\" stroke=\"black\" fill=\"none\" />");
    }

    public void DrawEllipse(double cx, double cy, double rx, double ry)
    {
        _elements.Add(
            $"<ellipse cx=\"{Fmt(cx)}\" cy=\"{Fmt(cy)}\" rx=\"{Fmt(rx)}\" ry=\"{Fmt(ry)}\" stroke=\"black\" fill=\"none\" />");
    }

    public string GetSvg()
    {
        var builder = new StringBuilder();
        builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">");

        foreach (var element in _elements)
        {
            builder.Append("  ");
            builder.AppendLine(element);
        }

        builder.AppendLine("</svg>");
        return builder.ToString();
    }

    private static string Fmt(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);
}
