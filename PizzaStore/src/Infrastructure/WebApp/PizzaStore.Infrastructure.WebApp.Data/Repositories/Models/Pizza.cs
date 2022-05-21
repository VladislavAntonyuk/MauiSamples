namespace PizzaStore.Infrastructure.WebApp.Data.Repositories.Models;

using Infrastructure.Data.Repositories.Models;

public class Pizza : BaseModel
{
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string? Image { get; set; }
	public decimal Price { get; set; }
}