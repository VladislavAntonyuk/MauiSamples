namespace MauiAnimation;

using CommunityToolkit.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			   .UseMauiCommunityToolkit()
			   .ConfigureFonts(fonts =>
				{
					fonts.AddFont("Ontel-8Mr62.otf", "Ontel");
				});
		return builder.Build();
	}
}