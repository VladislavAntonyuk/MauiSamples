namespace MauiMaps;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Maps.Handlers;

public static class MapExtensions
{
	public static async Task AddAnnotation(this CustomPin pin)
	{
		var googleMap = ((IMapHandler?)pin.Map?.Handler)?.Map;
		if (googleMap is not null)
		{
			var markerWithIcon = new MarkerOptions();
			markerWithIcon.SetPosition(new LatLng(pin.Location.Latitude, pin.Location.Longitude));
			markerWithIcon.SetTitle(pin.Label);
			markerWithIcon.SetSnippet(pin.Address);
			var imageSourceHandler = new ImageLoaderSourceHandler();
			var bitmap = await imageSourceHandler.LoadImageAsync(pin.ImageSource, Application.Context);
			markerWithIcon.SetIcon(bitmap is null
				                       ? BitmapDescriptorFactory.DefaultMarker()
				                       : BitmapDescriptorFactory.FromBitmap(bitmap));

			googleMap.SetOnMarkerClickListener(new CustomMarkerClickListener(bitmap));

			googleMap.AddMarker(markerWithIcon);
		}
	}
}