namespace PizzaStore.Application.Interfaces.CQRS;

public interface ICommandDispatcher
{
	Task<IOperationResult<TResult>> SendAsync<TResult, TCommand>(TCommand command, CancellationToken cancellationToken)
		where TCommand : ICommand<TResult>;
}