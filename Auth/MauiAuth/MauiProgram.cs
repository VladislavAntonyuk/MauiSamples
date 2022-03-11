using AuthServices;
using CommunityToolkit.Maui;

namespace MauiAuth;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Services.AddScoped<MainPage>();
        builder.Services.AddScoped<AzureB2CPage>();
        builder.Services.AddScoped<AzureADPage>();
        builder.Services.RegisterServices();
        builder.UseMauiCommunityToolkit();
        return builder.Build();
    }
}