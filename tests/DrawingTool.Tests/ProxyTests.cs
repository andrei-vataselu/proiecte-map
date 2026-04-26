using DrawingTool.Core.Interfaces;
using DrawingTool.Core.Proxy;
using DrawingTool.Core.Shapes;

namespace DrawingTool.Tests;

public class ProxyTests
{
    [Test]
    public void Move_OnReadOnlyProxy_ThrowsInvalidOperationException()
    {
        var proxy = new ReadOnlyShapeProxy(new Circle(10, 10, 5));

        Assert.Throws<InvalidOperationException>(() => proxy.Move(2, 3));
    }

    [Test]
    public void Draw_OnReadOnlyProxy_DelegatesWithoutThrowing()
    {
        var proxy = new ReadOnlyShapeProxy(new Circle(10, 10, 5));
        var canvas = new RecordingCanvas();

        Assert.DoesNotThrow(() => proxy.Draw(canvas));
        Assert.That(canvas.CirclesDrawn, Is.EqualTo(1));
    }

    private sealed class RecordingCanvas : ICanvas
    {
        public int CirclesDrawn { get; private set; }

        public void DrawLine(double x1, double y1, double x2, double y2)
        {
        }

        public void DrawCircle(double cx, double cy, double r)
        {
            CirclesDrawn++;
        }

        public void DrawRect(double x, double y, double w, double h)
        {
        }

        public void DrawEllipse(double cx, double cy, double rx, double ry)
        {
        }
    }
}
