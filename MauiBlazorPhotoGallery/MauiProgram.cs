namespace MauiBlazorPhotoGallery;
using MauiBlazorPhotoGallery.Data;
using Microsoft.AspNetCore.Components.WebView.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>();

        builder.Services.AddBlazorWebView();
        builder.Services.AddSingleton<MediaService>();

        return builder.Build();
    }
}
