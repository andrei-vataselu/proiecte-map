using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Shapes;

public sealed class Rectangle : IShape
{
    public Rectangle(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public double X { get; private set; }
    public double Y { get; private set; }
    public double Width { get; private set; }
    public double Height { get; private set; }

    public void Draw(ICanvas canvas) => canvas.DrawRect(X, Y, Width, Height);

    public void Move(double dx, double dy)
    {
        X += dx;
        Y += dy;
    }

    public void Scale(double factor)
    {
        X *= factor;
        Y *= factor;
        Width *= factor;
        Height *= factor;
    }

    public BoundingBox GetBoundingBox() => new(X, Y, X + Width, Y + Height);
}
