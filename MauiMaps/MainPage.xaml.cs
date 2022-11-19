namespace MauiMaps;

using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Maps;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		var customPinFromUri = new CustomPin()
		{
			Label = "From Uri",
			Location = new Location(10, 10),
			Address = "Address",
			ImageSource = ImageSource.FromUri(new Uri("https://picsum.photos/50")),
			Map = MyMap
		};
		var customPinFromResource = new CustomPin()
		{
			Label = "From Resource",
			Location = new Location(12, 12),
			Address = "Address3",
			ImageSource = ImageSource.FromResource("MauiMaps.Resources.EmbeddedImages.icon.jpeg"),
			Map = MyMap
		};
		var imageData = File.ReadAllBytes("dotnet_bot.png");
		var customPinFromStream = new CustomPin()
		{
			Label = "From Stream",
			Location = new Location(13, 13),
			Address = "Address4",
			ImageSource = ImageSource.FromStream(() => new MemoryStream(imageData)),
			Map = MyMap
		};
		MyMap.Pins.Add(customPinFromUri);
		MyMap.Pins.Add(customPinFromResource);
		MyMap.Pins.Add(customPinFromStream);
		customPinFromUri.InfoWindowClicked += async delegate
		{
			await Toast.Make("Info Window is clicked").Show();
		};
		MyMap.MoveToRegion(new MapSpan(new Location(10,10), 10, 10));
	}
}