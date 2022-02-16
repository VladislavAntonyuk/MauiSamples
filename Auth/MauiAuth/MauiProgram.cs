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
        builder.Services.AddSingleton<AuthServices.AuthService>();
        builder.UseMauiCommunityToolkit();
        return builder.Build();
    }
}