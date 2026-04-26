namespace DrawingTool.Core.Models;

public readonly record struct BoundingBox(double MinX, double MinY, double MaxX, double MaxY)
{
    public double Width => MaxX - MinX;
    public double Height => MaxY - MinY;

    public static BoundingBox Union(BoundingBox first, BoundingBox second)
    {
        return new BoundingBox(
            Math.Min(first.MinX, second.MinX),
            Math.Min(first.MinY, second.MinY),
            Math.Max(first.MaxX, second.MaxX),
            Math.Max(first.MaxY, second.MaxY));
    }
}
