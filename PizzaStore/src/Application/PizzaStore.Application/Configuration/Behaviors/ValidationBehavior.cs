namespace PizzaStore.Application.Configuration.Behaviors;

using FluentValidation;
using MediatR;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : class, IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		this.validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		if (!validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
		var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
		if (failures.Count != 0)
		{
			throw new ValidationException(failures);
		}

		return await next();
	}
}