namespace MauiSpeech;

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;

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

		return builder.Build();
	}
}