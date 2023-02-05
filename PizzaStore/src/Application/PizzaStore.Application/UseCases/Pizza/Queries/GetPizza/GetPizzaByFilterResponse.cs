namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizza;

public class GetPizzaByFilterResponse
{
	public List<PizzaDto> Items { get; set; } = new();
	public int PageIndex { get; }
	public int TotalPages { get; }
	public int TotalCount { get; }
}