namespace PizzaStore.Application.UseCases.Pizza.Commands.Create;

using AutoMapper;
using Domain.Entities;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class CreatePizzaCommandHandler : BasePizzaHandler, ICommandHandler<PizzaDto, CreatePizzaCommand>
{
	public CreatePizzaCommandHandler(IPizzaRepository pizzaRepository, IMapper mapper) : base(pizzaRepository, mapper)
	{
	}

	public async Task<IOperationResult<PizzaDto>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
	{
		var pizza = Mapper.Map<Pizza>(request);
		var result = await PizzaRepository.Add(pizza, cancellationToken);
		return new OperationResult<PizzaDto>
		{
			Value = Mapper.Map<PizzaDto>(result)
		};
	}
}