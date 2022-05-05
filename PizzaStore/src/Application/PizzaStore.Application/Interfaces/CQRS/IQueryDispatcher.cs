﻿namespace PizzaStore.Application.Interfaces.CQRS;

public interface IQueryDispatcher
{
	Task<IOperationResult<TResult>> SendAsync<TResult, TQuery>(TQuery query, CancellationToken cancellationToken = default)
		where TQuery : IQuery<TResult>;
}