namespace PizzaStore.Application.Interfaces.Repositories;

using Domain.Entities;

public interface IPizzaRepository
{
	Task<Pizza> Add(Pizza pizza, CancellationToken cancellationToken);
	void Update(Pizza pizza);
	void Delete(Pizza pizza);
	Task<Pizza?> GetById(int id, CancellationToken cancellationToken);
	Task<bool> IsExist(string parameter, CancellationToken cancellationToken);
	IEnumerable<Pizza> GetAll();
	Task<IPaginatedList<Pizza>> GetPagedAsync(string? parameter, int requestOffset, int requestLimit, CancellationToken cancellationToken);
}