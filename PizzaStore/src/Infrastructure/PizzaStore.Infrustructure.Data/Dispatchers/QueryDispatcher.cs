namespace PizzaStore.Infrastructure.Data.Dispatchers;

using Application.Interfaces.CQRS;
using MediatR;

public class QueryDispatcher : IQueryDispatcher
{
	private readonly ISender sender;

	public QueryDispatcher(ISender sender)
	{
		this.sender = sender;
	}

	public Task<IOperationResult<TResult>> SendAsync<TResult, TQuery>(TQuery query, CancellationToken cancellationToken)
		where TQuery : IQuery<TResult>
	{
		return sender.Send(query, cancellationToken);
	}
}