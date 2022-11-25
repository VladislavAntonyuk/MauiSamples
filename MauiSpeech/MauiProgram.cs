namespace MauiSpeech;

using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit();
		builder.Services.AddSingleton(TextToSpeech.Default);
		builder.Services.AddSingleton(SpeechToText.Default);

		builder.Services.AddSingleton<MainPage, MainViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}