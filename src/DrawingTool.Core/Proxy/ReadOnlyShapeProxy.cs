using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Proxy;

public sealed class ReadOnlyShapeProxy : IShape
{
    private readonly IShape _innerShape;

    public ReadOnlyShapeProxy(IShape innerShape)
    {
        _innerShape = innerShape;
    }

    public void Draw(ICanvas canvas) => _innerShape.Draw(canvas);

    public void Move(double dx, double dy) => throw new InvalidOperationException("Shape is locked");

    public void Scale(double factor) => throw new InvalidOperationException("Shape is locked");

    public BoundingBox GetBoundingBox() => _innerShape.GetBoundingBox();
}
