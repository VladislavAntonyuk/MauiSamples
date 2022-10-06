namespace PizzaStore.Application.Configuration.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<TRequest> logger;

	public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
	{
		this.logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		try
		{
			return await next();
		}
		catch (Exception ex)
		{
			var requestName = typeof(TRequest).Name;
			logger.LogError(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
			throw;
		}
	}
}