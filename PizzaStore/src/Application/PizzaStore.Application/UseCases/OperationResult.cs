namespace PizzaStore.Application.UseCases;

using System.Diagnostics.CodeAnalysis;
using Interfaces.CQRS;

public record OperationResult<T> : IOperationResult<T>
{
	public T? Value { get; init; }

	[MemberNotNullWhen(true, "Value")]
	public bool IsSuccessful => !Errors.Any();

	public ICollection<string> Errors { get; } = new List<string>();
}