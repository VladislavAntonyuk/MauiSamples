namespace MauiMaps;

using CommunityToolkit.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().UseMauiMaps()
		       .UseMauiCommunityToolkit();
		builder.ConfigureMauiHandlers(handlers =>
		{
#if ANDROID
			handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#elif IOS || MACCATALYST
			handlers.AddHandler<CustomPin, CustomPinHandler>();
#endif
		});

		return builder.Build();
	}
}