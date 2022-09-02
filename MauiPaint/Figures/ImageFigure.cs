namespace MauiPaint.Figures;

public class ImageFigure : IFigure
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Width { get; set; } = 100;
	public int Height { get; set; } = 100;
	public Stream ImageStream { get; set; } = Stream.Null;

	public void Draw(ICanvas canvas, RectF rectF)
	{
#if ANDROID || IOS || MACCATALYST
		ImageStream.Position = 0;
		var image = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(ImageStream);
		var newImage = image.Resize(Width, Height, ResizeMode.Stretch, true);
		canvas.DrawImage(newImage, X, Y, newImage.Width, newImage.Height);
#endif
	}

	public async Task Configure()
	{
		X = await FigureExtensions.SetParameter<int>("X");
		Y = await FigureExtensions.SetParameter<int>("Y");
		Width = await FigureExtensions.SetParameter<int>("Width");
		Height = await FigureExtensions.SetParameter<int>("Height");
		var path = await FigureExtensions.SetParameter<string>("Path");
		if (File.Exists(path))
		{
			ImageStream = new MemoryStream(await File.ReadAllBytesAsync(path));
		}
	}
}