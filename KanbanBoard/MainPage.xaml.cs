namespace KanbanBoard;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Application;
using Font = Microsoft.Maui.Font;

public partial class MainPage : ContentPage
{
	private readonly IPath path;

	public MainPage(MainPageViewModel viewModel, IPath path)
	{
		InitializeComponent();
		On<iOS>().SetUseSafeArea(true);
		BindingContext = viewModel;
		this.path = path;
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
		var dbPath = path.GetDatabasePath();
		path.DeleteFile(dbPath);
		Application.Current?.Quit();
	}
}