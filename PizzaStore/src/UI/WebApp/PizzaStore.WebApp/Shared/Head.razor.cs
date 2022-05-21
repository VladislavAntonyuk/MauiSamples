namespace PizzaStore.WebApp.Shared;

using Microsoft.AspNetCore.Components;

public partial class Head : PizzaStoreBaseComponent
{
	[Inject]
	private NavigationManager NavigationManager { get; set; } = null!;

	[Parameter]
	public string Title { get; set; } = "PizzaStore";

	[Parameter]
	public string Description { get; set; } = "PizzaStore";

	[Parameter]
	public string Image { get; set; } = "favicon.ico";

	[Parameter]
	public string Keywords { get; set; } = "PizzaStore";
}