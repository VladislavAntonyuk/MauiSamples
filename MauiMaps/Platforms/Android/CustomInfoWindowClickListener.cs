namespace MauiMaps;

using Android.Gms.Maps;

internal class CustomInfoWindowClickListener(CustomMapHandler mapHandler)
	: Java.Lang.Object, GoogleMap.IOnInfoWindowClickListener
{
	public void OnInfoWindowClick(Android.Gms.Maps.Model.Marker marker)
	{
		var pin = mapHandler.Markers.FirstOrDefault(x => x.marker.Id == marker.Id);
		pin.pin?.SendInfoWindowClick();
	}
}