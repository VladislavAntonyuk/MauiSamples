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
			var classToUpdate = Mapper.Map<Pizza>(command);
			classToUpdate.Price = classToUpdate.Price == 0 ? pizza.Price : classToUpdate.Price;
			var updatedClass = await PizzaRepository.Update(classToUpdate, cancellationToken);
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