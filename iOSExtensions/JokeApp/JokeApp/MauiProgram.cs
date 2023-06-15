namespace JokeApp;

using Refit;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddRefitClient<IJokeApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://joke.deno.dev"));

		return builder.Build();
	}
}