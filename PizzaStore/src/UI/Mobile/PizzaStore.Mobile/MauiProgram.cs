namespace PizzaStore.Mobile;

using Application.Configuration;
using CommunityToolkit.Maui;
using Infrastructure.Mobile.Business;
using Infrastructure.Mobile.Data.Configuration;
using Infrastructure.Mobile.Data.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using ViewModels;
using Views;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();
		builder.UseMauiCommunityToolkit(options =>
		{
			options.SetShouldSuppressExceptionsInAnimations(true);
			options.SetShouldSuppressExceptionsInBehaviors(true);
			options.SetShouldSuppressExceptionsInConverters(true);
		});
		builder.Services.AddApplication();
		builder.Services.AddInfrastructureData(GetDatabaseConnectionString("PizzaStore"));
		builder.Services.AddInfrastructureBusiness();
		builder.Services.AddSingleton<BasketViewModel>();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<BasketPage>();
		builder.Services.AddSingleton<MainPage>();
		var app = builder.Build();
		MigrateDb(app.Services);
		return app;
	}

	private static string GetDatabaseConnectionString(string filename)
	{
		return $"Filename={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename)}.db";
	}

	private static void MigrateDb(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MobileAppContext>>();
		using var context = factory.CreateDbContext();
		context.Database.Migrate();
		SeedData(context);
	}

	private static void SeedData(MobileAppContext context)
	{
		if (context.Pizza.Any())
		{
			return;
		}

		var pizzas = new[]
		{
			new Pizza()
			{
				Name = "Margherita",
				Price = 5.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/grumbling/pizza-hotone-pan.png"
			},
			new Pizza()
			{
				Name = "Pepperoni",
				Price = 6.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-halaal-hawaiian-thin.png"
			},
			new Pizza()
			{
				Name = "Hawaiian",
				Price = 7.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-hawaiian-pan.png"
			},
			new Pizza()
			{
				Name = "Vegetarian",
				Price = 8.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-vegetarian-pan.png"
			},
			new Pizza()
			{
				Name = "Meat Lovers",
				Price = 9.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/grumbling/pizza-bolognaise-pan.png"
			},
			new Pizza()
			{
				Name = "Supreme",
				Price = 10.99m,
				Image = "https://romans-bnry.s3.eu-west-2.amazonaws.com/images/root/v2/pizza/pizza-regina-pan.png"
			}
		};
		context.Pizza.AddRange(pizzas);
		context.SaveChanges();
	}
}