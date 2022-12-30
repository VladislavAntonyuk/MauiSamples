namespace MauiMaps;

using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Maps;

public partial class MainPage : ContentPage
{
	public ObservableCollection<Site> Sites { get; } = new();
	
	public MainPage()
	{
		InitializeComponent();
		InitMap1();
		InitMapMvvm();
	}

	void InitMap1()
	{
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
		MyMap.Pins.Add(customPinFromUri);
		MyMap.Pins.Add(customPinFromResource);
		customPinFromUri.InfoWindowClicked += async delegate
		{
			await Toast.Make("Info Window is clicked").Show();
		};
		customPinFromUri.MarkerClicked += async delegate
		{
			await Toast.Make("Marker is clicked").Show();
		};
		MyMap.MoveToRegion(new MapSpan(new Location(10, 10), 10, 10));
	}

	void InitMapMvvm()
	{
		BindingContext = this;
		var site1 = new Site()
		{
			Location = new Location(10, 10),
			Description = "From Uri",
			Address = "Address",
			ImageSource = ImageSource.FromUri(new Uri("https://picsum.photos/50")),
		};
		var site2 = new Site()
		{
			Description = "From Resource",
			Location = new Location(12, 12),
			Address = "Address3",
			ImageSource = ImageSource.FromResource("MauiMaps.Resources.EmbeddedImages.icon.jpeg")
		};
		Sites.Add(site1);
		Sites.Add(site2);
		MyMap2.MoveToRegion(new MapSpan(new Location(10, 10), 10, 10));
	}
}

public class Site
{
	public string? Description { get; set; }
	public string? Address { get; set; }
	public Location? Location { get; set; }
	public ImageSource? ImageSource { get; set; }
}