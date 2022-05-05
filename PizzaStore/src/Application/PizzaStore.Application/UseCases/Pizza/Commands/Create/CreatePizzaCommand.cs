namespace PizzaStore.Application.UseCases.Pizza.Commands.Create;

using Interfaces.CQRS;

public class CreatePizzaCommand : ICommand<PizzaDto>
{
	public string Name { get; init; } = string.Empty;
}
