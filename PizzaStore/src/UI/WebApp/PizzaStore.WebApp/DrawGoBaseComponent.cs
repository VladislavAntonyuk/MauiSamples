namespace PizzaStore.WebApp;

using I18nText;
using Microsoft.AspNetCore.Components;

public abstract class PizzaStoreBaseComponent : ComponentBase
{
	protected Translation Translation = new();

	[Inject]
	protected Toolbelt.Blazor.I18nText.I18nText I18NText { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		Translation = await I18NText.GetTextTableAsync<Translation>(this);
		await base.OnInitializedAsync();
	}
}