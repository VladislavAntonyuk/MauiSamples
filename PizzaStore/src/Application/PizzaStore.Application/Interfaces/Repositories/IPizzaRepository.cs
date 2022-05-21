namespace PizzaStore.Application.Interfaces.Repositories;

using Domain.Entities;

public interface IPizzaRepository
{
	Task<Pizza> Add(Pizza pizza, CancellationToken cancellationToken);
	Task<Pizza> Update(Pizza pizza, CancellationToken cancellationToken);
	Task Delete(Pizza pizza, CancellationToken cancellationToken);
	Task<Pizza?> GetById(int id, CancellationToken cancellationToken);
	Task<bool> IsExist(string parameter, CancellationToken cancellationToken);
	Task<IEnumerable<Pizza>> GetAll(CancellationToken cancellationToken);

	Task<IPaginatedList<Pizza>> GetPagedAsync(string? parameter,
		int requestOffset,
		int requestLimit,
		CancellationToken cancellationToken);
}