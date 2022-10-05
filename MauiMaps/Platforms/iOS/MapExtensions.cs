namespace MauiMaps;

using CoreLocation;
using MapKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Maps;

public static partial class MapExtensions
{
	public static async Task AddAnnotation(this CustomPin pin)
	{
		var imageSourceHandler = new ImageLoaderSourceHandler();
		var image = await imageSourceHandler.LoadImageAsync(pin.ImageSource);
		var annotation = new CustomAnnotation (pin.Label, new CLLocationCoordinate2D (pin.Location.Latitude, pin.Location.Longitude), image);

		var nativeMap = (MKMapView?)pin.Map?.Handler?.PlatformView;
		if (nativeMap is not null)
		{
			nativeMap.Delegate = new MapDelegate();
			nativeMap.AddAnnotation (annotation);
		}
	}
}