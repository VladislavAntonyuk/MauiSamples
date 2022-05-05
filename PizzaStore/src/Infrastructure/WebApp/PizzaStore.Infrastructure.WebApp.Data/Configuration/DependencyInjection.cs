namespace PizzaStore.Infrastructure.WebApp.Data.Configuration;

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
		services.AddDbContextFactory<WebAppContext>(opt => opt.UseMySql(
			                                            connectionString,
			                                            ServerVersion.AutoDetect(connectionString),
			                                            builder =>
			                                            {
				                                            builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
			                                            }));
		
		services.AddAutoMapper(typeof(DependencyInjection));

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddHostedService<MigrationHostedService>();
	}
}