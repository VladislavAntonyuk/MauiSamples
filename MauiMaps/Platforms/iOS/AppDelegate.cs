using Foundation;

namespace MauiMaps;

using CoreGraphics;
using CoreLocation;
using MapKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using UIKit;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

class MapDelegate : MKMapViewDelegate
{
	UIImageView? venueView;
	UIImage? venueImage;
	static string annotationId = "ConferenceAnnotation";
	public override MKAnnotationView? GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
	{
		MKAnnotationView? annotationView = null;

		if (annotation is MKUserLocation)
			return null;

		if (annotation is ConferenceAnnotation) {

			// show conference annotation
			annotationView = mapView.DequeueReusableAnnotation (annotationId);

			if (annotationView == null)
				annotationView = new MKAnnotationView (annotation, annotationId);

			annotationView.Image = UIImage.FromFile ("images/conference.png");
			annotationView.CanShowCallout = true;
		}

		return annotationView;
	}

	public override void DidSelectAnnotationView (MKMapView mapView, MKAnnotationView view)
	{
		// show an image view when the conference annotation view is selected
		if (view.Annotation is ConferenceAnnotation) {

			venueView = new UIImageView ();
			venueView.ContentMode = UIViewContentMode.ScaleAspectFit;
			venueImage = UIImage.FromFile ("image/venue.png");
			venueView.Image = venueImage;
			view.AddSubview (venueView);

			UIView.Animate (0.4, () => {
				venueView.Frame = new CGRect (-75, -75, 200, 200); });
		}
	}

	public override void DidDeselectAnnotationView (MKMapView mapView, MKAnnotationView view)
	{
		// remove the image view when the conference annotation is deselected
		if (view.Annotation is ConferenceAnnotation) {

			venueView?.RemoveFromSuperview ();
			venueView?.Dispose ();
			venueView = null;
		}
	}
}

public class ConferenceAnnotation : MKAnnotation
{
	string title;
	CLLocationCoordinate2D coord;

	public ConferenceAnnotation (string title,
		CLLocationCoordinate2D coord)
	{
		this.title = title;
		this.coord = coord;
	}

	public override string Title {
		get {
			return title;
		}
	}

	public override CLLocationCoordinate2D Coordinate {
		get {
			return coord;
		}
	}
}

public static class MapExtensions
{
	public static void AddConferenceAnnotation (this IMap map)
	{
		var annotation = new ConferenceAnnotation ("Xamarin Evolve 2014",
			new CLLocationCoordinate2D (47.6204, -122.3491));

		var nativeMap = (MKMapView?)map.Handler?.PlatformView;
		if (nativeMap is not null)
		{
			nativeMap.Delegate = new MapDelegate();
			nativeMap.AddAnnotation (annotation);
		}
	}

}