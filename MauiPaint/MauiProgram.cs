namespace MauiPaint;

using CommunityToolkit.Maui;
using Platforms.Services;
using Serializer;
using Services;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit();

		builder.Services.AddSingleton<ISerializerService, JsonSerializerService>();
		builder.Services.AddSingleton(DeviceInfo.Current);
		builder.Services.AddSingleton<IDialogService, DialogService>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}