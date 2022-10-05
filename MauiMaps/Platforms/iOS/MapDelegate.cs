namespace MauiMaps;

using CoreGraphics;
using MapKit;
using UIKit;

class MapDelegate : MKMapViewDelegate
{
	UIImageView? venueView;
	UIImage? venueImage;
	static string annotationId = "CustomAnnotation";
	public override MKAnnotationView? GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
	{
		MKAnnotationView? annotationView = null;

		if (annotation is CustomAnnotation customAnnotation)
		{
			annotationView = mapView.DequeueReusableAnnotation (annotationId) ?? new MKAnnotationView (annotation, annotationId);
			annotationView.Image = customAnnotation.Image;
			annotationView.CanShowCallout = true;
		}

		return annotationView;
	}

	public override void DidSelectAnnotationView (MKMapView mapView, MKAnnotationView view)
	{
		if (view.Annotation is CustomAnnotation customAnnotation) {

			venueView = new UIImageView ();
			venueView.ContentMode = UIViewContentMode.ScaleAspectFit;
			venueImage = customAnnotation.Image;
			venueView.Image = venueImage;
			view.AddSubview (venueView);

			UIView.Animate (0.4, () => {
				venueView.Frame = new CGRect (-75, -75, 200, 200);
			});
		}
	}

	public override void DidDeselectAnnotationView (MKMapView mapView, MKAnnotationView view)
	{
		if (view.Annotation is CustomAnnotation) {

			venueView?.RemoveFromSuperview ();
			venueView?.Dispose ();
			venueView = null;
		}
	}
}