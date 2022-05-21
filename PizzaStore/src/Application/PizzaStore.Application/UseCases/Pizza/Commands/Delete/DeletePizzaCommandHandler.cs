namespace PizzaStore.Application.UseCases.Pizza.Commands.Delete;

using AutoMapper;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class DeletePizzaCommandHandler : BasePizzaHandler, ICommandHandler<bool, DeletePizzaCommand>
{
	public DeletePizzaCommandHandler(IPizzaRepository pizzaRepository, IMapper mapper) : base(pizzaRepository, mapper)
	{
	}

	public async Task<IOperationResult<bool>> Handle(DeletePizzaCommand command, CancellationToken cancellationToken)
	{
		var pizza = await PizzaRepository.GetById(command.PizzaId, cancellationToken);
		if (pizza is not null)
		{
			await PizzaRepository.Delete(pizza, cancellationToken);
			return new OperationResult<bool>
			{
				Value = true
			};
		}

		var result = new OperationResult<bool>();
		result.Errors.Add("Pizza not found");
		return result;
	}
}