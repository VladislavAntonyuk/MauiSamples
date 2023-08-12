namespace MauiMaps;

using Android.Gms.Maps;
using Microsoft.Maui.Maps;

class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
{
	private readonly CustomMapHandler mapHandler;

	public MapCallbackHandler(CustomMapHandler mapHandler)
	{
		this.mapHandler = mapHandler;
	}

	public void OnMapReady(GoogleMap googleMap)
	{
		mapHandler.UpdateValue(nameof(IMap.Pins));
		mapHandler.Map?.SetOnMarkerClickListener(new CustomMarkerClickListener(mapHandler));
		mapHandler.Map?.SetOnInfoWindowClickListener(new CustomInfoWindowClickListener(mapHandler));
	}
}