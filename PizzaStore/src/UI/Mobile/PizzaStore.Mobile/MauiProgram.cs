namespace PizzaStore.Mobile;

using Application.Configuration;
using CommunityToolkit.Maui;
using Infrastructure.Mobile.Business;
using Infrastructure.Mobile.Data.Configuration;
using Infrastructure.Mobile.Data.Repositories.Models;
using Microsoft.EntityFrameworkCore;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
		       .UseMauiCommunityToolkit();
		builder.Services.AddApplication();
		builder.Services.AddInfrastructureData(GetDatabaseConnectionString("PizzaStore"));
		builder.Services.AddInfrastructureBusiness();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<MainPage>();
		var app = builder.Build();
		MigrateDb(app.Services);
		return app;
	}

	private static string GetDatabaseConnectionString(string filename)
	{
		return $"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename)}.db";
	}

	static void MigrateDb(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MobileAppContext>>();
		using var context = factory.CreateDbContext();
		context.Database.Migrate();
	}
}
