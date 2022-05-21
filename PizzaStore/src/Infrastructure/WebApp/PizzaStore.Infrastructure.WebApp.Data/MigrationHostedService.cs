namespace PizzaStore.Infrastructure.WebApp.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories.Models;

internal class MigrationHostedService : IHostedService
{
	private readonly IServiceProvider serviceProvider;

	public MigrationHostedService(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await using var scope = serviceProvider.CreateAsyncScope();
		var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<WebAppContext>>();
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		await context.Database.MigrateAsync(cancellationToken);
		await SeedData(context, cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	static async Task SeedData(WebAppContext context, CancellationToken cancellationToken)
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
		await context.Pizza.AddRangeAsync(pizzas, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);
	}
}