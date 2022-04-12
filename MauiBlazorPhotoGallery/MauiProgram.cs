namespace MauiBlazorPhotoGallery;
using Data;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddSingleton<MediaService>();

        return builder.Build();
    }
}
