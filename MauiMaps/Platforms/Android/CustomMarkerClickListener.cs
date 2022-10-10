namespace MauiMaps;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;

public class CustomMarkerClickListener : Java.Lang.Object, GoogleMap.IOnMarkerClickListener
{
	private readonly Bitmap? bitmap;

	public CustomMarkerClickListener(Bitmap? bitmap)
	{
		this.bitmap = bitmap;
	}

	public bool OnMarkerClick(Marker marker)
	{
		marker.HideInfoWindow();
		marker.SetIcon(bitmap is null
			               ? BitmapDescriptorFactory.DefaultMarker()
			               : BitmapDescriptorFactory.FromBitmap(bitmap));
		marker.ShowInfoWindow();
		return true;
	}
}