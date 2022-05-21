namespace PizzaStore.Application.UseCases.Pizza;

using AutoMapper;
using Interfaces.Repositories;

public abstract class BasePizzaHandler
{
	protected readonly IPizzaRepository PizzaRepository;
	protected readonly IMapper Mapper;

	protected BasePizzaHandler(IPizzaRepository pizzaRepository, IMapper mapper)
	{
		PizzaRepository = pizzaRepository;
		Mapper = mapper;
	}
}