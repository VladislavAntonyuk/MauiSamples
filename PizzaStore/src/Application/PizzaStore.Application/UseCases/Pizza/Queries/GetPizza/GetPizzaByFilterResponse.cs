namespace PizzaStore.Application.UseCases.Pizza.Queries.GetPizza;

public class GetPizzaByFilterResponse
{
	public IReadOnlyCollection<PizzaDto> Items { get; set; } = new List<PizzaDto>();
	public int PageIndex { get; }
	public int TotalPages { get; }
	public int TotalCount { get; }
}