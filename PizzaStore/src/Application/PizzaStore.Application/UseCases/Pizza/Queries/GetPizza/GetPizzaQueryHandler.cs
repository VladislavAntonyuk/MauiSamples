namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizza;

using AutoMapper;
using Interfaces.CQRS;
using Interfaces.Repositories;

public class GetPizzaQueryHandler : BasePizzaHandler, IQueryHandler<GetPizzaByFilterResponse, GetPizzaQuery>
{
	public GetPizzaQueryHandler(IPizzaRepository pizzaRepository, IMapper mapper) : base(pizzaRepository, mapper)
	{
	}

	public async Task<IOperationResult<GetPizzaByFilterResponse>> Handle(GetPizzaQuery request, CancellationToken cancellationToken)
	{
		var result = await PizzaRepository.GetPagedAsync(request.Name, request.Offset, request.Limit, cancellationToken);
		return new OperationResult<GetPizzaByFilterResponse>
		{
			Value = Mapper.Map<GetPizzaByFilterResponse>(result)
		};
	}
}