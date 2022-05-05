namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizzaById;

using AutoMapper;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class GetPizzaByIdQueryHandler : BasePizzaHandler, IQueryHandler<PizzaDto, GetPizzaByIdQuery>
{
	public GetPizzaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
	{
	}

	public async Task<IOperationResult<PizzaDto>> Handle(GetPizzaByIdQuery request, CancellationToken cancellationToken)
	{
		var pizza = await UnitOfWork.PizzaRepository.GetById(request.Id, cancellationToken);
		if (pizza is not null)
		{
			return new OperationResult<PizzaDto>
			{
				Value = Mapper.Map<PizzaDto>(pizza)
			};
		}

		var result = new OperationResult<PizzaDto>();
		result.Errors.Add("Pizza not found");
		return result;

	}
}
