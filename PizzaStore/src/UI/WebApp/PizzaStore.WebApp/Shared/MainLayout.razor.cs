namespace PizzaStore.WebApp.Shared;

using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class MainLayout : LayoutComponentBase
{
	private MudThemeProvider? mudThemeProvider;
	private bool isDarkMode = true;
	bool drawerOpen = true;
	private bool rightToLeft;
	
	void DrawerToggle()
	{
		drawerOpen = !drawerOpen;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			if (mudThemeProvider is not null)
			{
				isDarkMode = await mudThemeProvider.GetSystemPreference();
			}
		}

		await base.OnAfterRenderAsync(firstRender);
	}
}
