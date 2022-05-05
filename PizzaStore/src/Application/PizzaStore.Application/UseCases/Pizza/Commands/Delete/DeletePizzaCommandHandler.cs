namespace PizzaStore.Application.UseCases.Pizza.Commands.Delete;

using AutoMapper;
using Domain.Entities;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class DeletePizzaCommandHandler : BasePizzaHandler, ICommandHandler<bool, DeletePizzaCommand>
{
	public DeletePizzaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
	{
	}

	public async Task<IOperationResult<bool>> Handle(DeletePizzaCommand command, CancellationToken cancellationToken = default)
	{
		var pizza = Mapper.Map<Pizza>(command);
		UnitOfWork.PizzaRepository.Delete(pizza);
		await UnitOfWork.Save(cancellationToken);
		return new OperationResult<bool>
		{
			Value = true
		};
	}
}
