namespace MauiMaps;

using Android.Gms.Maps;

internal class CustomMarkerClickListener : Java.Lang.Object, GoogleMap.IOnMarkerClickListener
{
	private readonly CustomMapHandler mapHandler;

	public CustomMarkerClickListener(CustomMapHandler mapHandler)
	{
		this.mapHandler = mapHandler;
	}

	public bool OnMarkerClick(Android.Gms.Maps.Model.Marker marker)
	{
		var pin = mapHandler.Markers.FirstOrDefault(x => x.marker.Id == marker.Id);
		pin.pin?.SendMarkerClick();
		marker.ShowInfoWindow();
		return true;
	}
}