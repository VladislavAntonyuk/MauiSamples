namespace PizzaStore.Infrastructure.WebApp.Business;

using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PizzaStore.WebApp.Application;

public static class DependencyInjection
{
	public static void AddInfrastructureBusiness(this IServiceCollection services)
	{
		services.AddScoped<IPaymentService, CreditCardPaymentService>();
		services.AddScoped<IServiceInterface1, ServicePizza>();
	}
}