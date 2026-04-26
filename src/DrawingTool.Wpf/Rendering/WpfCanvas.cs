using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingTool.Core.Interfaces;

namespace DrawingTool.Wpf.Rendering;

public sealed class WpfCanvas : ICanvas
{
    private readonly Canvas _canvas;

    public WpfCanvas(Canvas canvas)
    {
        _canvas = canvas;
        _canvas.Children.Clear();
    }

    public void DrawLine(double x1, double y1, double x2, double y2)
    {
        var line = new Line
        {
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        _canvas.Children.Add(line);
    }

    public void DrawCircle(double cx, double cy, double r)
    {
        var ellipse = new Ellipse
        {
            Width = r * 2,
            Height = r * 2,
            Stroke = Brushes.DarkBlue,
            Fill = Brushes.Transparent,
            StrokeThickness = 2
        };

        Canvas.SetLeft(ellipse, cx - r);
        Canvas.SetTop(ellipse, cy - r);
        _canvas.Children.Add(ellipse);
    }

    public void DrawRect(double x, double y, double w, double h)
    {
        var rectangle = new Rectangle
        {
            Width = w,
            Height = h,
            Stroke = Brushes.DarkGreen,
            Fill = Brushes.Transparent,
            StrokeThickness = 2
        };

        Canvas.SetLeft(rectangle, x);
        Canvas.SetTop(rectangle, y);
        _canvas.Children.Add(rectangle);
    }

    public void DrawEllipse(double cx, double cy, double rx, double ry)
    {
        var ellipse = new Ellipse
        {
            Width = rx * 2,
            Height = ry * 2,
            Stroke = Brushes.DarkRed,
            Fill = Brushes.Transparent,
            StrokeThickness = 2
        };

        Canvas.SetLeft(ellipse, cx - rx);
        Canvas.SetTop(ellipse, cy - ry);
        _canvas.Children.Add(ellipse);
    }
}
