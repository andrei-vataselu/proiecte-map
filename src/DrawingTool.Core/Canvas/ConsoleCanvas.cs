using DrawingTool.Core.Interfaces;

namespace DrawingTool.Core.Canvas;

public sealed class ConsoleCanvas : ICanvas
{
    public void DrawLine(double x1, double y1, double x2, double y2)
    {
        Console.WriteLine($"DrawLine({x1}, {y1}, {x2}, {y2})");
    }

    public void DrawCircle(double cx, double cy, double r)
    {
        Console.WriteLine($"DrawCircle({cx}, {cy}, {r})");
    }

    public void DrawRect(double x, double y, double w, double h)
    {
        Console.WriteLine($"DrawRect({x}, {y}, {w}, {h})");
    }

    public void DrawEllipse(double cx, double cy, double rx, double ry)
    {
        Console.WriteLine($"DrawEllipse({cx}, {cy}, {rx}, {ry})");
    }
}
