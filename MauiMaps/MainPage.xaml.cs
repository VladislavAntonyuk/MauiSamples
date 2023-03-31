namespace MauiMaps;

using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Maps;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
		InitMap1();
	}

	void InitMap1()
	{
		var pinKyiv = new CustomPin()
		{
			Label = "Kyiv",
			Location = new Location(50.45466, 30.5238),
			Address = "Ukraine",
			ImageSource = ImageSource.FromUri(new Uri("https://www.gamesatlas.com/images/football/teams/ukraine/dynamo-kyiv.png"))
		};
		var pinSimferopol = new CustomPin()
		{
			Label = "Simferopol",
			Location = new Location(44.95719, 34.11079),
			Address = "Crimea, Ukraine",
			ImageSource = ImageSource.FromResource("MauiMaps.Resources.EmbeddedImages.Tavriya.png")
		};
		MyMap.Pins.Add(pinKyiv);
		MyMap.Pins.Add(pinSimferopol);
		pinKyiv.InfoWindowClicked += async delegate
		{
			await Toast.Make("The capital of Ukraine").Show();
		};
		pinSimferopol.MarkerClicked += async delegate
		{
			await Toast.Make("Welcome to Crimea").Show();
		};
		MyMap.MoveToRegion(new MapSpan(new Location(47, 31), 10, 15));
	}
}