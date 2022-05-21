namespace PizzaStore.Infrastructure.WebApp.Business;

using PizzaStore.WebApp.Application;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static void AddInfrastructureBusiness(this IServiceCollection services)
	{
		services.AddScoped<IPaymentService, CreditCardPaymentService>();
		services.AddScoped<IServiceInterface1, ServicePizza>();
	}
}