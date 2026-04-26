using System;
using System.Windows;
using DrawingTool.Core.Canvas;
using DrawingTool.Core.Composite;
using DrawingTool.Core.Proxy;
using DrawingTool.Core.Shapes;
using DrawingTool.Wpf.Rendering;

namespace DrawingTool.Wpf;

public partial class MainWindow : Window
{
    private Picture _scene = new();

    public MainWindow()
    {
        InitializeComponent();
        BuildScene();
        Render();
    }

    private void BuildSceneClick(object sender, RoutedEventArgs e)
    {
        BuildScene();
        Render();
        StatusText.Text = "Scene rebuilt.";
    }

    private void MoveSceneClick(object sender, RoutedEventArgs e)
    {
        _scene.Move(20, 10);
        Render();
        StatusText.Text = "Scene moved by (20, 10).";
    }

    private void ScaleSceneClick(object sender, RoutedEventArgs e)
    {
        _scene.Scale(1.2);
        Render();
        StatusText.Text = "Scene scaled by 1.2.";
    }

    private void ProxyDemoClick(object sender, RoutedEventArgs e)
    {
        var proxy = new ReadOnlyShapeProxy(new Circle(120, 80, 25));
        try
        {
            proxy.Move(10, 0);
        }
        catch (InvalidOperationException ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    private void ExportSvgClick(object sender, RoutedEventArgs e)
    {
        var svgCanvas = new SvgCanvas();
        _scene.Draw(svgCanvas);
        SvgTextBox.Text = svgCanvas.GetSvg();
        StatusText.Text = "SVG generated.";
    }

    private void BuildScene()
    {
        var root = new Picture();
        root.Add(new Line(30, 30, 160, 110));
        root.Add(new Rectangle(180, 40, 160, 90));

        var nested = new Picture();
        nested.Add(new Circle(130, 220, 40));
        nested.Add(new Ellipse(270, 230, 70, 35));

        var deepNested = new Picture();
        deepNested.Add(new Rectangle(360, 190, 120, 80));
        deepNested.Add(new Line(350, 280, 500, 340));

        nested.Add(deepNested);
        root.Add(nested);
        _scene = root;
    }

    private void Render()
    {
        var canvas = new WpfCanvas(ViewportCanvas);
        _scene.Draw(canvas);
    }
}
