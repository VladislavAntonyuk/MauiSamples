using CommunityToolkit.Maui;
using KanbanBoard.Db;
using KanbanBoard.Models;

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
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
			});
		builder.UseMauiCommunityToolkit();

		builder.Services.AddSingleton<IPath, DbPath>();
		builder.Services.AddDbContext<KanbanBoardDbContext>();
		builder.Services.AddSingleton<IColumnsRepository, ColumnsRepository>();
		builder.Services.AddSingleton<ICardsRepository, CardsRepository>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}