namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizzaById;

using Interfaces.CQRS;

public class GetPizzaByIdQuery : IQuery<PizzaDto>
{
	public GetPizzaByIdQuery(int id)
	{
		Id = id;
	}

	public int Id { get; }
}