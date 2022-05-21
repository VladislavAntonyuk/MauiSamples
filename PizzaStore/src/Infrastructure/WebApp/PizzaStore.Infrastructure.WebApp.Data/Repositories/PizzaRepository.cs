namespace PizzaStore.Infrastructure.WebApp.Data.Repositories;

using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases;
using AutoMapper;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;
using DomainPizza = Domain.Entities.Pizza;

public class PizzaRepository : BaseRepository, IPizzaRepository
{
	private readonly IDbContextFactory<WebAppContext> factory;
	private readonly IMapper mapper;

	public PizzaRepository(IDbContextFactory<WebAppContext> factory, IMapper mapper)
	{
		this.factory = factory;
		this.mapper = mapper;
	}

	public async Task<DomainPizza> Add(DomainPizza pizza, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var newPizza = mapper.Map<Pizza>(pizza);
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		await context.Pizza.AddAsync(newPizza, cancellationToken);
		await SaveChangesAsync(context, cancellationToken);
		return mapper.Map<DomainPizza>(newPizza);
	}

	public async Task<DomainPizza> Update(DomainPizza pizza, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var pizzaToUpdate = mapper.Map<Pizza>(pizza);
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		context.Pizza.Update(pizzaToUpdate);
		await SaveChangesAsync(context, cancellationToken);
		return mapper.Map<DomainPizza>(pizzaToUpdate);
	}

	public async Task Delete(DomainPizza pizza, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var pizzaToRemove = mapper.Map<Pizza>(pizza);
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		context.Pizza.Remove(pizzaToRemove);
		await SaveChangesAsync(context, cancellationToken);
	}

	public async Task<DomainPizza?> GetById(int id, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		var pizza = await context.Pizza.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
		return mapper.Map<DomainPizza>(pizza);
	}

	public async Task<bool> IsExist(string parameter, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		return await context.Pizza.AnyAsync(x => x.Name == parameter, cancellationToken);
	}

	public async Task<IEnumerable<DomainPizza>> GetAll(CancellationToken cancellationToken)
	{
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		var pizza = context.Pizza.AsNoTracking().ToListAsync(cancellationToken);
		return mapper.Map<IEnumerable<DomainPizza>>(pizza);
	}

	public async Task<IPaginatedList<DomainPizza>> GetPagedAsync(string? requestName,
		int requestOffset,
		int requestLimit,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		var totalCount = await context.Pizza.AsNoTracking()
									  .CountAsync(x => x.Name.Contains(requestName ?? string.Empty), cancellationToken);

		var result = await context.Pizza.AsNoTracking()
								  .Where(x => x.Name.Contains(requestName ?? string.Empty))
								  .OrderBy(q => q.Id)
								  .Skip(requestOffset)
								  .Take(requestLimit)
								  .ToListAsync(cancellationToken);

		return new PaginatedList<DomainPizza>(mapper.Map<List<DomainPizza>>(result), totalCount, requestOffset,
											   requestLimit);
	}

	public async Task<DomainPizza?> GetByName(string name, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await using var context = await factory.CreateDbContextAsync(cancellationToken);
		var pizza = await context.Pizza.AsNoTracking()
								  .SingleOrDefaultAsync(x => x.Name.Contains(name), cancellationToken);
		return mapper.Map<DomainPizza>(pizza);
	}
}