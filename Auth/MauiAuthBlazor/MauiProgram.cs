namespace MauiAuthBlazor;
using AuthServices;
using Microsoft.AspNetCore.Components.WebView.Maui;
using CommunityToolkit.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureEssentials();
            
        builder.Services.AddBlazorWebView();
        builder.Services.RegisterServices();
        builder.UseMauiCommunityToolkit();

        return builder.Build();
    }
}