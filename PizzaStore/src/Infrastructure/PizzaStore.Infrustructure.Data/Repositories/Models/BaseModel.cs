namespace PizzaStore.Infrastructure.Data.Repositories.Models;

public abstract class BaseModel
{
	public int Id { get; set; }
	public DateTime CreatedOn { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public string? ModifiedBy { get; set; }
}