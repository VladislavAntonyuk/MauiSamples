namespace PizzaStore.Domain;

public abstract class BaseEntity
{
	public int Id { get; set; }

	public string? CreatedBy { get; set; }

	public DateTime CreatedOn { get; set; }

	public string? ModifiedBy { get; set; }

	public DateTime? ModifiedOn { get; set; }
}