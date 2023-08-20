namespace MauiMaps;

using Android.Gms.Maps;

internal class CustomInfoWindowClickListener : Java.Lang.Object, GoogleMap.IOnInfoWindowClickListener
{
	private readonly CustomMapHandler mapHandler;

	public CustomInfoWindowClickListener(CustomMapHandler mapHandler)
	{
		this.mapHandler = mapHandler;
	}

	public void OnInfoWindowClick(Android.Gms.Maps.Model.Marker marker)
	{
		var pin = mapHandler.Markers.FirstOrDefault(x => x.marker.Id == marker.Id);
		pin.pin?.SendInfoWindowClick();
	}
}