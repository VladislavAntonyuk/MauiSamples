namespace MauiPaint.Figures;

public static class FigureFactory
{
	public static async Task<IFigure> CreateFigure(string name)
	{
		IFigure figure = name switch
		{
			"Ellipse" => new Ellipse(),
			"Rectangle" => new Rectangle(),
			"Text" => new TextFigure(),
			"Image" => new ImageFigure(),
			_ => throw new NotSupportedException()
		};
		await figure.Configure();
		return figure;
	}
}