using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Shapes;

public sealed class Ellipse : IShape
{
    public Ellipse(double centerX, double centerY, double radiusX, double radiusY)
    {
        CenterX = centerX;
        CenterY = centerY;
        RadiusX = radiusX;
        RadiusY = radiusY;
    }

    public double CenterX { get; private set; }
    public double CenterY { get; private set; }
    public double RadiusX { get; private set; }
    public double RadiusY { get; private set; }

    public void Draw(ICanvas canvas) => canvas.DrawEllipse(CenterX, CenterY, RadiusX, RadiusY);

    public void Move(double dx, double dy)
    {
        CenterX += dx;
        CenterY += dy;
    }

    public void Scale(double factor)
    {
        CenterX *= factor;
        CenterY *= factor;
        RadiusX *= factor;
        RadiusY *= factor;
    }

    public BoundingBox GetBoundingBox()
    {
        return new BoundingBox(
            CenterX - RadiusX,
            CenterY - RadiusY,
            CenterX + RadiusX,
            CenterY + RadiusY);
    }
}
