namespace PizzaStore.Application.UseCases.Pizza.Commands.Update;

using AutoMapper;
using Domain.Entities;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class UpdatePizzaCommandHandler : BasePizzaHandler, ICommandHandler<PizzaDto, UpdatePizzaCommand>
{
	public UpdatePizzaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
	{
	}

	public async Task<IOperationResult<PizzaDto>> Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
	{
		var banner = Mapper.Map<Pizza>(request);
		UnitOfWork.PizzaRepository.Update(banner);
		await UnitOfWork.Save(cancellationToken);
		return new OperationResult<PizzaDto>
		{
			Value = Mapper.Map<PizzaDto>(banner)
		};
	}
}
