namespace MauiPaint.Figures;

public class TextFigure : IFigure
{
	public int X { get; set; }
	public int Y { get; set; }
	public string? Text { get; set; }

	public void Draw(ICanvas canvas, RectF rectF)
	{
		canvas.DrawString(Text ?? string.Empty, X, Y, HorizontalAlignment.Center);
	}

	public async Task Configure()
	{
		X = await FigureExtensions.SetParameter<int>("X");
		Y = await FigureExtensions.SetParameter<int>("Y");
		Text = await FigureExtensions.SetParameter<string>("Text");
	}
}