namespace MauiIpCamera;

using CommunityToolkit.Maui;
using ViewModels;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCamera();

		builder.Services.AddSingleton<CameraViewModel>();
		builder.Services.AddSingleton<SettingsViewModel>();

		builder.Services.AddSingleton<ILocalIpService, LocalIpService>();
		builder.Services.AddSingleton<IAutoStartService, AutoStartService>();
		builder.Services.AddSingleton(Preferences.Default);

		return builder.Build();
	}
}