using ConfigCat.Client;
using Microsoft.Extensions.Logging;

namespace MauiDynamicConfiguration;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddSingleton<UserContext>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton(s => ConfigCatClient.Get("YOUR_KEY_HERE",
														options =>
														{
															options.PollingMode = PollingModes.AutoPoll(pollInterval: TimeSpan.FromSeconds(95));
															if (options.Logger != null)
															{
																options.Logger.LogLevel =
																	ConfigCat.Client.LogLevel.Info;
															}
														}));
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}