using DrawingTool.Core.Canvas;
using DrawingTool.Core.Shapes;

namespace DrawingTool.Tests;

public class BridgeTests
{
    [Test]
    public void SvgCanvas_AfterCircleDraw_ContainsCircleTag()
    {
        var canvas = new SvgCanvas();
        var circle = new Circle(25, 30, 10);

        circle.Draw(canvas);
        var svg = canvas.GetSvg();

        Assert.That(svg, Does.Contain("<circle"));
    }
}
