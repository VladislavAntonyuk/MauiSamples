using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using task_list_app_maui.Data;
using task_list_app_maui.ViewModels;
using task_list_app_maui.Views;

namespace task_list_app_maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("task-list-app-maui"); });
		
		builder.Services.AddSingleton<TarefasViewModel>();
		
		builder.Services.AddSingleton<TarefasPendentesView>();
		builder.Services.AddSingleton<TarefasConcluidasView>();

		return builder.Build();
	}
}
