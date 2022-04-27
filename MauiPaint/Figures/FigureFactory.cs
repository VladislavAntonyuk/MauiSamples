namespace MauiPaint.Figures;

public class FigureFactory
{
	public static async Task<IFigure> CreateFigure(string name)
	{
		IFigure figure = name switch
		{
			"Ellipse" => new Ellipse(),
			"Rectangle" => new Rectangle(),
			_ => throw new NotSupportedException()
		};
		await figure.Configure();
		return figure;
	}
}