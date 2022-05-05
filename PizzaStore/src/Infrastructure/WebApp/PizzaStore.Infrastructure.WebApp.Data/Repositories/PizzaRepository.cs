namespace PizzaStore.Infrastructure.WebApp.Data.Repositories;

using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using DomainPizza = Domain.Entities.Pizza;

public class PizzaRepository : IPizzaRepository
{
	private readonly WebAppContext context;
	private readonly IMapper mapper;

	public PizzaRepository(WebAppContext context, IMapper mapper)
	{
		this.context = context;
		this.mapper = mapper;
	}

	public async Task<DomainPizza> Add(DomainPizza pizza, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var newPizza = mapper.Map<Pizza>(pizza);

		var resultPizza = await context.Pizza.AddAsync(newPizza, cancellationToken);

		return mapper.Map<DomainPizza>(resultPizza.Entity);
	}

	public void Update(DomainPizza pizza)
	{
		var pizzaToUpdate = mapper.Map<Pizza>(pizza);
		context.Pizza.Update(pizzaToUpdate);
	}

	public void Delete(DomainPizza pizza)
	{
		var pizzaToRemove = mapper.Map<Pizza>(pizza);
		context.Pizza.Remove(pizzaToRemove);
	}

	public async Task<DomainPizza?> GetById(int id, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var pizza = await context.Pizza.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
		return mapper.Map<DomainPizza>(pizza);
	}

	public Task<bool> IsExist(string parameter, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		return context.Pizza.AnyAsync(x => x.Name == parameter, cancellationToken);
	}

	public async Task<DomainPizza?> GetByName(string name, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var pizza = await context.Pizza.AsNoTracking().SingleOrDefaultAsync(x => x.Name.Contains(name), cancellationToken);
		return mapper.Map<DomainPizza>(pizza);
	}

	public IEnumerable<DomainPizza> GetAll()
	{
		var pizza = context.Pizza.AsNoTracking().AsAsyncEnumerable();
		return mapper.Map<IEnumerable<DomainPizza>>(pizza);
	}

	public async Task<IPaginatedList<DomainPizza>> GetPagedAsync(string? requestName, int requestOffset, int requestLimit, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var totalCount = await context.Pizza
									  .AsNoTracking()
									  .CountAsync(x => x.Name.Contains(requestName ?? string.Empty), cancellationToken);

		var result = await context.Pizza
						   .AsNoTracking()
						   .Where(x => x.Name.Contains(requestName ?? string.Empty))
						   .OrderBy(q => q.Id)
						   .Skip(requestOffset)
						   .Take(requestLimit)
						   .ToListAsync(cancellationToken);

		return new PaginatedList<DomainPizza>(mapper.Map<List<DomainPizza>>(result), totalCount, requestOffset,
											   requestLimit);
	}
}
