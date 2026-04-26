using DrawingTool.Core.Composite;
using DrawingTool.Core.Shapes;

namespace DrawingTool.Tests;

public class CompositeTests
{
    [Test]
    public void Scale_OnPicture_PropagatesToAllChildren()
    {
        var line = new Line(0, 0, 10, 10);
        var circle = new Circle(10, 10, 5);
        var rectangle = new Rectangle(5, 5, 20, 10);
        var picture = new Picture();
        picture.Add(line);
        picture.Add(circle);
        picture.Add(rectangle);

        picture.Scale(2.0);

        Assert.Multiple(() =>
        {
            Assert.That(line.X2, Is.EqualTo(20));
            Assert.That(line.Y2, Is.EqualTo(20));
            Assert.That(circle.CenterX, Is.EqualTo(20));
            Assert.That(circle.CenterY, Is.EqualTo(20));
            Assert.That(circle.Radius, Is.EqualTo(10));
            Assert.That(rectangle.X, Is.EqualTo(10));
            Assert.That(rectangle.Y, Is.EqualTo(10));
            Assert.That(rectangle.Width, Is.EqualTo(40));
            Assert.That(rectangle.Height, Is.EqualTo(20));
        });
    }

    [Test]
    public void GetBoundingBox_OnPicture_ReturnsUnifiedBounds()
    {
        var picture = new Picture();
        picture.Add(new Circle(10, 10, 5));
        picture.Add(new Rectangle(20, 4, 10, 6));
        picture.Add(new Line(0, 30, 8, 22));

        var box = picture.GetBoundingBox();

        Assert.Multiple(() =>
        {
            Assert.That(box.MinX, Is.EqualTo(0));
            Assert.That(box.MinY, Is.EqualTo(4));
            Assert.That(box.MaxX, Is.EqualTo(30));
            Assert.That(box.MaxY, Is.EqualTo(30));
        });
    }
}
