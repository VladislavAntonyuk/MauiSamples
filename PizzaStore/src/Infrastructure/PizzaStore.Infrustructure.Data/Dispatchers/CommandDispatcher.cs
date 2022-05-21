namespace PizzaStore.Infrastructure.Data.Dispatchers;

using Application.Interfaces.CQRS;
using MediatR;

public class CommandDispatcher : ICommandDispatcher
{
	private readonly ISender sender;

	public CommandDispatcher(ISender sender)
	{
		this.sender = sender;
	}

	public Task<IOperationResult<TResult>> SendAsync<TResult, TCommand>(TCommand command, CancellationToken cancellationToken)
		where TCommand : ICommand<TResult>
	{
		return sender.Send(command, cancellationToken);
	}
}