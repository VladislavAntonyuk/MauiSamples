namespace PizzaStore.Application.UseCases.Pizza.Commands.Update;

using FluentValidation;
using Interfaces.Repositories;

public class UpdatePizzaCommandValidator : AbstractValidator<UpdatePizzaCommand>
{
	private readonly IUnitOfWork unitOfWork;

	public UpdatePizzaCommandValidator(IUnitOfWork unitOfWork)
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