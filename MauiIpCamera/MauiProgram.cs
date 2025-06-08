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
		builder.Services.AddSingleton<ILocalIpService, LocalIpService>();

		return builder.Build();
	}
}