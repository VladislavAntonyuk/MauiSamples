using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MauiMaps;

using Microsoft.Maui.Maps.Handlers;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize |
                                 ConfigChanges.Orientation |
                                 ConfigChanges.UiMode |
                                 ConfigChanges.ScreenLayout |
                                 ConfigChanges.SmallestScreenSize |
                                 ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}

public static class MapExtensions
{
	public static void AddConferenceAnnotation (this Microsoft.Maui.Maps.IMap map)
	{
		var map = ((IMapHandler?)map.Handler).Map;
		if (map is not null)
		{
			foreach (var mapHandler in map.Pins)
			{
				var markerWithIcon = new MarkerOptions ();

				markerWithIcon.SetPosition (new LatLng (formsPin.Position.Latitude, formsPin.Position.Longitude));
				markerWithIcon.SetTitle (formsPin.Label);
				markerWithIcon.SetSnippet (formsPin.Address);

				if (!string.IsNullOrEmpty (formsPin.PinIcon))
					markerWithIcon.InvokeIcon (BitmapDescriptorFactory.FromAsset (String.Format ("{0}.png", formsPin.PinIcon)));
				else
					markerWithIcon.InvokeIcon (BitmapDescriptorFactory.DefaultMarker ());

				androidMapView.Map.AddMarker (markerWithIcon);
			}
		}
	}

}