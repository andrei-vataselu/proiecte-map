using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Models;

namespace DrawingTool.Core.Composite;

public sealed class Picture : IShape
{
    private readonly List<IShape> _shapes = new();

    public IReadOnlyCollection<IShape> Shapes => _shapes;

    public void Add(IShape shape) => _shapes.Add(shape);

    public bool Remove(IShape shape) => _shapes.Remove(shape);

    public void Draw(ICanvas canvas)
    {
        foreach (var shape in _shapes)
        {
            shape.Draw(canvas);
        }
    }

    public void Move(double dx, double dy)
    {
        foreach (var shape in _shapes)
        {
            shape.Move(dx, dy);
        }
    }

    public void Scale(double factor)
    {
        foreach (var shape in _shapes)
        {
            shape.Scale(factor);
        }
    }

    public BoundingBox GetBoundingBox()
    {
        if (_shapes.Count == 0)
        {
            return new BoundingBox(0, 0, 0, 0);
        }

        var current = _shapes[0].GetBoundingBox();
        for (var i = 1; i < _shapes.Count; i++)
        {
            current = BoundingBox.Union(current, _shapes[i].GetBoundingBox());
        }

        return current;
    }
}
