namespace DynamicFab;

using CommunityToolkit.Maui.Alerts;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnFolderClicked(object sender, EventArgs e)
	{
		await Toast.Make("On Folder clicked").Show();
	}

	private async void OnWordClicked(object sender, EventArgs e)
	{
		await Toast.Make("On Word clicked").Show();
	}

	private async void OnExcelClicked(object sender, EventArgs e)
	{
		await Toast.Make("On Excel clicked").Show();
	}
}