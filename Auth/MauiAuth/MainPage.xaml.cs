namespace MauiAuth;

public partial class MainPage : ContentPage
{
	private readonly AzureADPage _azureAdPage;
	private readonly AzureB2CPage _azureB2CPage;

	public MainPage(AzureADPage azureAdPage, AzureB2CPage azureB2CPage)
	{
		_azureAdPage = azureAdPage;
		_azureB2CPage = azureB2CPage;
		InitializeComponent();
	}

	async void AzureADPageClicked(object? sender, EventArgs args)
	{
		await Navigation.PushAsync(_azureAdPage);
	}

	async void AzureB2CPageClicked(object? sender, EventArgs args)
	{
		await Navigation.PushAsync(_azureB2CPage);
	}

}