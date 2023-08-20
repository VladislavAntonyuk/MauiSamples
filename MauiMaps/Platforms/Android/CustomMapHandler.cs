namespace MauiMaps;

using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;

public class CustomMapHandler : MapHandler
{
	public static readonly IPropertyMapper<IMap, IMapHandler> CustomMapper =
		new PropertyMapper<IMap, IMapHandler>(Mapper)
		{
			[nameof(IMap.Pins)] = MapPins
		};

	public CustomMapHandler() : base(CustomMapper, CommandMapper)
	{
	}

	public CustomMapHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null) : base(
		mapper ?? CustomMapper, commandMapper ?? CommandMapper)
	{
	}

	public List<(IMapPin pin, Marker marker)> Markers { get; } = new();

	protected override void ConnectHandler(MapView platformView)
	{
		base.ConnectHandler(platformView);
		var mapReady = new MapCallbackHandler(this);
		PlatformView.GetMapAsync(mapReady);
	}

	private static new void MapPins(IMapHandler handler, IMap map)
	{
		if (handler is CustomMapHandler mapHandler)
		{
			var pinsToAdd = map.Pins.Where(x => x.MarkerId == null).ToList();
			var pinsToRemove = mapHandler.Markers.Where(x => !map.Pins.Contains(x.pin)).ToList();
			foreach (var marker in pinsToRemove)
			{
				marker.marker.Remove();
				mapHandler.Markers.Remove(marker);
			}

			mapHandler.AddPins(pinsToAdd);
		}
	}

	private void AddPins(IEnumerable<IMapPin> mapPins)
	{
		if (Map is null || MauiContext is null)
		{
			return;
		}

		foreach (var pin in mapPins)
		{
			var pinHandler = pin.ToHandler(MauiContext);
			if (pinHandler is IMapPinHandler mapPinHandler)
			{
				var markerOption = mapPinHandler.PlatformView;
				if (pin is CustomPin cp)
				{
					cp.ImageSource.LoadImage(MauiContext, result =>
					{
						if (result?.Value is BitmapDrawable { Bitmap: not null } bitmapDrawable)
						{
							markerOption.SetIcon(BitmapDescriptorFactory.FromBitmap(GetMaximumBitmap(bitmapDrawable.Bitmap, 100, 100)));
						}

						AddMarker(Map, pin, markerOption);
					});
				}
				else
				{
					AddMarker(Map, pin, markerOption);
				}
			}
		}
	}

	private void AddMarker(GoogleMap map, IMapPin pin, MarkerOptions markerOption)
	{
		var marker = map.AddMarker(markerOption);
		pin.MarkerId = marker.Id;
		Markers.Add((pin, marker));
	}

	private static Bitmap GetMaximumBitmap(in Bitmap sourceImage, in float maxWidth, in float maxHeight)
	{
		var sourceSize = new Size(sourceImage.Width, sourceImage.Height);
		var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

		var width = Math.Max(maxResizeFactor * sourceSize.Width, 1);
		var height = Math.Max(maxResizeFactor * sourceSize.Height, 1);
		return Bitmap.CreateScaledBitmap(sourceImage, (int)width, (int)height, false)
				?? throw new InvalidOperationException("Failed to create Bitmap");
	}
}