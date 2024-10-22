using CommunityToolkit.Maui;
using KanbanBoard.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace KanbanBoard;

using KanbanBoardDb;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue31751", true);
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FASolid");
			});
		builder.UseMauiCommunityToolkit(x =>
		{
			x.SetShouldEnableSnackbarOnWindows(true);
		});

		builder.Services.AddDbContext<KanbanBoardDbContext>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}