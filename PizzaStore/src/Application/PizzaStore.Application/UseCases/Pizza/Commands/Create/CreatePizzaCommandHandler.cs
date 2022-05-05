namespace PizzaStore.Application.UseCases.Pizza.Commands.Create;

using AutoMapper;
using Domain.Entities;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class CreatePizzaCommandHandler : BasePizzaHandler, ICommandHandler<PizzaDto, CreatePizzaCommand>
{
	public CreatePizzaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
	{
	}

	public async Task<IOperationResult<PizzaDto>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
	{
		var banner = Mapper.Map<Pizza>(request);
		var result = await UnitOfWork.PizzaRepository.Add(banner, cancellationToken);
		await UnitOfWork.Save(cancellationToken);
		return new OperationResult<PizzaDto>
		{
			Value = Mapper.Map<PizzaDto>(result)
		};
	}
}
