namespace PizzaStore.Infrastructure.Mobile.Data.Configuration;

using System.Reflection;
using Application.Interfaces.Repositories;
using Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Models;

public static class DependencyInjection
{
	public static void AddInfrastructureData(this IServiceCollection services, string connectionString)
	{
		services.AddCommonInfrastructureData();
		services.AddPooledDbContextFactory<MobileAppContext>(opt => opt.UseSqlite(connectionString, builder =>
		{
			builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
		}));

		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddScoped<IPizzaRepository, PizzaRepository>();
	}
}