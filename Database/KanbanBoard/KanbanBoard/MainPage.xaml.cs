namespace KanbanBoard;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using KanbanBoardDb;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Application;
using Font = Microsoft.Maui.Font;

public partial class MainPage : ContentPage
{
	private readonly IServiceProvider serviceProvider;
	public MainPageViewModel ViewModel { get; }

	public MainPage(MainPageViewModel viewModel, IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
		BindingContext = ViewModel = viewModel;
		InitializeComponent();
		On<iOS>().SetUseSafeArea(true);
	}

	private async void ResetButton_OnClicked(object sender, EventArgs e)
	{
		var options = new SnackbarOptions
		{
			BackgroundColor = Colors.Red,
			TextColor = Colors.Green,
			CharacterSpacing = 1,
			ActionButtonFont = Font.SystemFontOfSize(14),
			ActionButtonTextColor = Colors.Yellow,
			CornerRadius = new CornerRadius(10),
			Font = Font.SystemFontOfSize(14),
		};
		await ResetButton.DisplaySnackbar(
			"All your data will be deleted. Application will be closed",
			DeleteDbAndCloseApp,
			"Confirm and delete",
			TimeSpan.FromSeconds(5),
			options);
	}

	private void DeleteDbAndCloseApp()
	{
		using var scope = serviceProvider.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<KanbanBoardDbContext>();
		dbContext.Database.EnsureDeleted();
#if WINDOWS
		System.Diagnostics.Process.Start("explorer","kanbanboard://");
#endif
		Application.Current?.Quit();
	}
}