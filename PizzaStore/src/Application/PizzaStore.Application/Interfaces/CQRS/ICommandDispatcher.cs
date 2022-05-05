namespace PizzaStore.Application.Interfaces.CQRS;

public interface ICommandDispatcher
{
	Task<IOperationResult<TResult>> SendAsync<TResult, TCommand>(TCommand command, CancellationToken cancellationToken = default)
		where TCommand : ICommand<TResult>;
}