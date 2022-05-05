namespace PizzaStore.Application.Interfaces.Repositories;

public interface IUnitOfWork : IAsyncDisposable
{
	IPizzaRepository PizzaRepository { get; }
	Task Save(CancellationToken cancellationToken);
	Task BeginTransaction(CancellationToken cancellationToken);
}