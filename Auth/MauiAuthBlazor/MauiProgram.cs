using AuthServices;
using Microsoft.AspNetCore.Components.WebView.Maui;
using CommunityToolkit.Maui;

namespace MauiAuthBlazor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureEssentials()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
            
        builder.Services.AddBlazorWebView();
        builder.Services.RegisterServices();
        builder.UseMauiCommunityToolkit();

        return builder.Build();
    }
}