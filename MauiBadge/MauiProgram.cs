using Microsoft.Extensions.Logging;

namespace MauiBadge;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton(NotificationCounter.Default);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}