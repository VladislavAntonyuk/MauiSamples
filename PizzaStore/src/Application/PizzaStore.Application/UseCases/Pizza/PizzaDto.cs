namespace PizzaStore.Application.UseCases.Pizza;

public class PizzaDto
{
	public int Id { get; set; }

	public string CreatedBy { get; set; } = string.Empty;

	public DateTime CreatedOn { get; set; }

	public string? ModifiedBy { get; set; }

	public DateTime? ModifiedOn { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Image { get; set; }
	public decimal Price { get; set; }
}