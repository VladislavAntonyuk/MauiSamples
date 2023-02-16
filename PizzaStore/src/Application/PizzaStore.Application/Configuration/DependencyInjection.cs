namespace PizzaStore.Application.Configuration;

using System.Reflection;
using Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
	}
}