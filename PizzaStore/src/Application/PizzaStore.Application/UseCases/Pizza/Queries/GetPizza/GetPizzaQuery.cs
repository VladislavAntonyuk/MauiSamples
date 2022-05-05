namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizza;

using Interfaces.CQRS;

public class GetPizzaQuery : IQuery<GetPizzaByFilterResponse>
{
	public string? Name { get; set; }
	public int Limit { get; set; }
	public int Offset { get; set; }
}