namespace PizzaStore.Infrastructure.Data.Configuration;

using Application.Interfaces.CQRS;
using Dispatchers;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static void AddCommonInfrastructureData(this IServiceCollection services)
	{
		services.AddTransient<ICommandDispatcher, CommandDispatcher>();
		services.AddTransient<IQueryDispatcher, QueryDispatcher>();
	}
}