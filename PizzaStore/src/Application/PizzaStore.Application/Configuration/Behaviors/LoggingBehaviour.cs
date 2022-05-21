namespace PizzaStore.Application.Configuration.Behaviors;

using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
	private readonly ILogger<TRequest> logger;

	public LoggingBehaviour(ILogger<TRequest> logger)
	{
		this.logger = logger;
	}

	public Task Process(TRequest request, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		logger.LogInformation("Request: {Name} {@Request}", requestName, request);
		return Task.CompletedTask;
	}
}