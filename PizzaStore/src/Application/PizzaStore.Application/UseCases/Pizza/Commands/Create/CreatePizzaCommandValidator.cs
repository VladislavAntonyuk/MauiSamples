namespace PizzaStore.Application.UseCases.Pizza.Commands.Create;

using FluentValidation;
using Interfaces.Repositories;

public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
	private readonly IUnitOfWork unitOfWork;

	public CreatePizzaCommandValidator(IUnitOfWork unitOfWork)
	{
		this.unitOfWork = unitOfWork;

		ConfigureValidation();
	}

	private void ConfigureValidation()
	{
		RuleFor(x => x.Price).GreaterThan(0);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MustAsync(async (command, name, ctx, cancellationToken) =>
			{
				var isExist = await unitOfWork.PizzaRepository.IsExist(name, cancellationToken);

				if (!isExist)
				{
					return true;
				}

				ctx.AddFailure(nameof(command.Name), $"Pizza with Name:'{command.Name}' already exist");
				return false;
			});
	}
}