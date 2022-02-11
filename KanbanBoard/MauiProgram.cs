using CommunityToolkit.Maui;
using KanbanBoard.Db;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace KanbanBoard;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("FontAwesome5Solid.otf", "FASolid");
            });
        builder.UseMauiCommunityToolkit();
        builder.Services.AddSingleton<IPath, DbPath>();
        builder.Services.AddSingleton<IColumnsRepository, ColumnsRepository>();
        builder.Services.AddSingleton<ICardsRepository, CardsRepository>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
        return builder.Build();
    }
}
