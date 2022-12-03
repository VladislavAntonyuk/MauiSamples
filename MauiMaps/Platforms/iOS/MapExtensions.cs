namespace MauiMaps;

using System;
using CoreLocation;
using MapKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Platform;
using UIKit;

public static partial class MapExtensions
{
	private static UIView? lastTouchedView;

	public static async Task AddAnnotation(this CustomPin pin)
	{
		var imageSourceHandler = new ImageLoaderSourceHandler();
		var image = await imageSourceHandler.LoadImageAsync(pin.ImageSource);
		var annotation = new CustomAnnotation()
		{
			Identifier = pin.Id,
			Image = image,
			Title = pin.Label,
			Subtitle = pin.Address,
			Coordinate = new CLLocationCoordinate2D(pin.Location.Latitude, pin.Location.Longitude),
			Pin = pin
		};
		pin.MarkerId = annotation;

		var nativeMap = (MauiMKMapView?)pin.Map?.Handler?.PlatformView;
		if (nativeMap is not null)
		{
			var customAnnotations = nativeMap.Annotations.OfType<CustomAnnotation>().Where(x => x.Identifier == annotation.Identifier).ToArray();
			nativeMap.RemoveAnnotations(customAnnotations);
			nativeMap.GetViewForAnnotation += GetViewForAnnotations;
			nativeMap.AddAnnotation(annotation);
		}
	}

	private static void OnCalloutClicked(IMKAnnotation annotation)
	{
		var pin = GetPinForAnnotation(annotation);
		if (lastTouchedView is MKAnnotationView)
			return;
		pin?.SendInfoWindowClick();
	}

	private static MKAnnotationView GetViewForAnnotations(MKMapView mapView, IMKAnnotation annotation)
	{
		MKAnnotationView? annotationView = null;

		if (annotation is CustomAnnotation customAnnotation)
		{
			annotationView = mapView.DequeueReusableAnnotation(customAnnotation.Identifier.ToString()) ??
							 new MKAnnotationView(annotation, customAnnotation.Identifier.ToString());
			annotationView.Image = customAnnotation.Image;
			annotationView.CanShowCallout = true;
		}

		var result = annotationView ?? new MKAnnotationView(annotation, null);
		AttachGestureToPin(result, annotation);
		return result;
	}

	static void AttachGestureToPin(MKAnnotationView mapPin, IMKAnnotation annotation)
	{
		var recognizers = mapPin.GestureRecognizers;

		if (recognizers != null)
		{
			foreach (var r in recognizers)
			{
				mapPin.RemoveGestureRecognizer(r);
			}
		}

		var recognizer = new UITapGestureRecognizer(g => OnCalloutClicked(annotation))
		{
			ShouldReceiveTouch = (gestureRecognizer, touch) =>
			{
				lastTouchedView = touch.View;
				return true;
			}
		};

		mapPin.AddGestureRecognizer(recognizer);
	}

	static IMapPin? GetPinForAnnotation(IMKAnnotation? annotation)
	{
		if (annotation is CustomAnnotation customAnnotation)
		{
			return customAnnotation.Pin;
		}

		return null;
	}
}