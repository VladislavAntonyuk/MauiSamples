namespace PizzaStore.Application.UseCases.Pizza.Commands.Update;

using Interfaces.CQRS;

public class UpdatePizzaCommand : ICommand<PizzaDto>
{
	public UpdatePizzaCommand(int id)
	{
		Id = id;
	}

	public int Id { get; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Image { get; set; }
	public decimal? Price { get; set; }
}