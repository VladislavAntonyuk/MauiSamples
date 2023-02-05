namespace MauiPaint.Figures;

public class Ellipse : IFigure
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Width { get; set; } = 100;
	public int Height { get; set; } = 100;

	public void Draw(ICanvas canvas, RectF rectF)
	{
		canvas.DrawEllipse(X, Y, Width, Height);
	}

	public async Task Configure()
	{
		X = await FigureExtensions.SetParameter<int>("X");
		Y = await FigureExtensions.SetParameter<int>("Y");
		Width = await FigureExtensions.SetParameter<int>("Width");
		Height = await FigureExtensions.SetParameter<int>("Height");
	}
}