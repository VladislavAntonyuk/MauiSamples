namespace MauiMaps;

using Android.App;
using Android.Gms.Maps.Model;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;

public static class MapExtensions
{
	public static async Task AddAnnotation(this CustomPin customPin)
	{
		var mapHandler = (IMapHandler?)customPin.Map?.Handler;
		var googleMap = mapHandler?.Map;
		if (mapHandler is not null && googleMap is not null && customPin.Map is not null)
		{
			var markerOptions = new List<MarkerOptions>();
			foreach (var pin in customPin.Map.Pins)
			{
				var markerWithIcon = new MarkerOptions();
				markerWithIcon.SetPosition(new LatLng(pin.Location.Latitude, pin.Location.Longitude));
				markerWithIcon.SetTitle(pin.Label);
				markerWithIcon.SetSnippet(pin.Address);
				if (pin is CustomPin cp)
				{
					var imageSourceHandler = new ImageLoaderSourceHandler();
					var bitmap = await imageSourceHandler.LoadImageAsync(cp.ImageSource, Application.Context);
					markerWithIcon.SetIcon(bitmap is null
						                       ? BitmapDescriptorFactory.DefaultMarker()
						                       : BitmapDescriptorFactory.FromBitmap(bitmap));

					googleMap.SetOnMarkerClickListener(new CustomMarkerClickListener(bitmap));
				}

				markerOptions.Add(markerWithIcon);
			}

			mapHandler.PlatformView.GetMapAsync(new MapCallbackHandler(markerOptions));
		}
	}
}