namespace AppContainer;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnClicked(object sender, EventArgs e)
	{
		await Launcher.OpenAsync("https://vladislavantonyuk.azurewebsites.net/");
	}
}