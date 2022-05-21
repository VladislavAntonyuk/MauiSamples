namespace PizzaStore.Application.UseCases.Pizza.Commands.Update;

using FluentValidation;
using Interfaces.Repositories;

public class UpdatePizzaCommandValidator : AbstractValidator<UpdatePizzaCommand>
{
	private readonly IPizzaRepository pizzaRepository;

	public UpdatePizzaCommandValidator(IPizzaRepository pizzaRepository)
	{
		this.pizzaRepository = pizzaRepository;

		ConfigureValidation();
	}

	private void ConfigureValidation()
	{
		RuleFor(x => x.Price)
			.GreaterThan(0)
			.When(x => x.Price is not null);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MustAsync(async (command, name, ctx, cancellationToken) =>
			{
				var isExist = await pizzaRepository.IsExist(name, cancellationToken);

				if (!isExist)
				{
					return true;
				}

				ctx.AddFailure(nameof(command.Name), $"Pizza with Name:'{command.Name}' already exist");
				return false;
			});
	}
}