namespace PizzaStore.Application.UseCases.Pizza.Commands.Update;

using Interfaces.CQRS;

public class UpdatePizzaCommand : ICommand<PizzaDto>
{
	public UpdatePizzaCommand(int pizzaId)
	{
		PizzaId = pizzaId;
	}

	public int PizzaId { get; }
	public string Name { get; init; } = string.Empty;
}
