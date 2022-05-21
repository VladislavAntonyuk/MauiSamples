namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizzaById;

using AutoMapper;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class GetPizzaByIdQueryHandler : BasePizzaHandler, IQueryHandler<PizzaDto, GetPizzaByIdQuery>
{
	public GetPizzaByIdQueryHandler(IPizzaRepository pizzaRepository, IMapper mapper) : base(pizzaRepository, mapper)
	{
	}

	public async Task<IOperationResult<PizzaDto>> Handle(GetPizzaByIdQuery request, CancellationToken cancellationToken)
	{
		var pizza = await PizzaRepository.GetById(request.Id, cancellationToken);
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