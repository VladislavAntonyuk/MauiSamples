namespace MauiAuthBlazor;
using AuthServices;
using CommunityToolkit.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().ConfigureEssentials();

		builder.Services.AddMauiBlazorWebView();
		builder.Services.RegisterServices();
		builder.UseMauiCommunityToolkit();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		return builder.Build();
	}
}