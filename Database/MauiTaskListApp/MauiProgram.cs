namespace MauiTaskListApp;

using CommunityToolkit.Maui;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels;
using Views;

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

		builder.Services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("MauiTaskListApp"); });

		builder.Services.AddSingleton<TasksViewModel>();

		builder.Services.AddSingleton<PendingTasksView>();
		builder.Services.AddSingleton<CompletedTasksView>();

		return builder.Build();
	}
}