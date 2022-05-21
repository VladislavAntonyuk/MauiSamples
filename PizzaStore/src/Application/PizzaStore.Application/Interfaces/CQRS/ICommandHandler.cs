namespace PizzaStore.Application.Interfaces.CQRS;

using MediatR;

public interface ICommandHandler<TResult, in TCommand> : IRequestHandler<TCommand, IOperationResult<TResult>>
	where TCommand : ICommand<TResult>
{
}