namespace MauiMaps;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;

class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
{
	private readonly IList<MarkerOptions> markerOptions;

	public MapCallbackHandler(IList<MarkerOptions> markerOptions)
	{
		this.markerOptions = markerOptions;
	}

	public void OnMapReady(GoogleMap googleMap)
	{
		googleMap.Clear();
		foreach (var markerOptions in markerOptions)
		{
			googleMap.AddMarker(markerOptions);
		}
	}
}