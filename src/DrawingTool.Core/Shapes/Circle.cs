using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Shapes;

public sealed class Circle : IShape
{
    public Circle(double centerX, double centerY, double radius)
    {
        CenterX = centerX;
        CenterY = centerY;
        Radius = radius;
    }

    public double CenterX { get; private set; }
    public double CenterY { get; private set; }
    public double Radius { get; private set; }

    public void Draw(ICanvas canvas) => canvas.DrawCircle(CenterX, CenterY, Radius);

    public void Move(double dx, double dy)
    {
        CenterX += dx;
        CenterY += dy;
    }

    public void Scale(double factor)
    {
        CenterX *= factor;
        CenterY *= factor;
        Radius *= factor;
    }

    public BoundingBox GetBoundingBox()
    {
        return new BoundingBox(
            CenterX - Radius,
            CenterY - Radius,
            CenterX + Radius,
            CenterY + Radius);
    }
}
