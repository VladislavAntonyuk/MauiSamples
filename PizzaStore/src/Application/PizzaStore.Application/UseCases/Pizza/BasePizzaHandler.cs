namespace PizzaStore.Application.UseCases.Pizza;

using AutoMapper;
using Interfaces.Repositories;

public abstract class BasePizzaHandler
{
	protected readonly IUnitOfWork UnitOfWork;
	protected readonly IMapper Mapper;

	protected BasePizzaHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		this.UnitOfWork = unitOfWork;
		this.Mapper = mapper;
	}
}