namespace DrawingTool.Core.Interfaces;

public interface ICanvas
{
    void DrawLine(double x1, double y1, double x2, double y2);
    void DrawCircle(double cx, double cy, double r);
    void DrawRect(double x, double y, double w, double h);
    void DrawEllipse(double cx, double cy, double rx, double ry);
}
