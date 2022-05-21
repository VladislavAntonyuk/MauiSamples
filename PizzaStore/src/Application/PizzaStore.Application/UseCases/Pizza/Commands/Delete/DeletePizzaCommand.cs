namespace PizzaStore.Application.UseCases.Pizza.Commands.Delete;

using Interfaces.CQRS;

public class DeletePizzaCommand : ICommand<bool>
{
	public DeletePizzaCommand(int pizzaId)
	{
		PizzaId = pizzaId;
	}

	public int PizzaId { get; }
}