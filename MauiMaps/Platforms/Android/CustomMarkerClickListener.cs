namespace MauiMaps;

using Android.Gms.Maps;

internal class CustomMarkerClickListener(CustomMapHandler mapHandler)
	: Java.Lang.Object, GoogleMap.IOnMarkerClickListener
{
	public bool OnMarkerClick(Android.Gms.Maps.Model.Marker marker)
	{
		var pin = mapHandler.Markers.FirstOrDefault(x => x.marker.Id == marker.Id);
		pin.pin?.SendMarkerClick();
		marker.ShowInfoWindow();
		return true;
	}
}