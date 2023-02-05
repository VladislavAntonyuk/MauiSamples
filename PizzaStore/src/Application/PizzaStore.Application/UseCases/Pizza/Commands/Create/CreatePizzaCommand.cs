namespace PizzaStore.Application.UseCases.Pizza.Commands.Create;

using Interfaces.CQRS;

public class CreatePizzaCommand : ICommand<PizzaDto>
{
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Image { get; set; }
	public decimal Price { get; set; }
}