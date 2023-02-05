namespace MauiPaint;

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Serializer;

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
		builder.Services.AddSingleton(FileSaver.Default);
		builder.Services.AddSingleton(FilePicker.Default);
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}