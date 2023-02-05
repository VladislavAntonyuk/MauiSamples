namespace PizzaStore.Application.UseCases.Pizza.Commands.Update;

using AutoMapper;
using Domain.Entities;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class UpdatePizzaCommandHandler : BasePizzaHandler, ICommandHandler<PizzaDto, UpdatePizzaCommand>
{
	public UpdatePizzaCommandHandler(IPizzaRepository pizzaRepository, IMapper mapper) : base(pizzaRepository, mapper)
	{
	}

	public async Task<IOperationResult<PizzaDto>> Handle(UpdatePizzaCommand command, CancellationToken cancellationToken)
	{
		var pizza = await PizzaRepository.GetById(command.Id, cancellationToken);
		if (pizza is not null)
		{
			var pizzaToUpdate = Mapper.Map<Pizza>(command);
			pizzaToUpdate.Price = pizzaToUpdate.Price == default ? pizza.Price : pizzaToUpdate.Price;
			pizzaToUpdate.Image ??= pizza.Image;
			pizzaToUpdate.Description ??= pizza.Description;
			pizzaToUpdate.CreatedBy = pizza.CreatedBy;
			pizzaToUpdate.CreatedOn = pizza.CreatedOn;
			var updatedClass = await PizzaRepository.Update(pizzaToUpdate, cancellationToken);
			return new OperationResult<PizzaDto>
			{
				Value = Mapper.Map<PizzaDto>(updatedClass)
			};
		}

		var result = new OperationResult<PizzaDto>();
		result.Errors.Add("Pizza not found");
		return result;
	}
}