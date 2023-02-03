namespace PizzaStore.Application.Configuration.Behaviors;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<TRequest> logger;
	private readonly Stopwatch timer;

	public PerformanceBehaviour(ILogger<TRequest> logger)
	{
		timer = new Stopwatch();

		this.logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		timer.Start();

		var response = await next();

		timer.Stop();

		var elapsedMilliseconds = timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 1000)
		{
			var requestName = typeof(TRequest).Name;
			logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
							  requestName, elapsedMilliseconds, request);
		}

		return response;
	}
}