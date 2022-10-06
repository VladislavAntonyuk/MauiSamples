namespace MauiMaps;

using CoreLocation;
using MapKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Maps.Platform;

public static partial class MapExtensions {
  public static async Task AddAnnotation(this CustomPin pin) {
    var imageSourceHandler = new ImageLoaderSourceHandler();
    var image = await imageSourceHandler.LoadImageAsync(pin.ImageSource);
    var annotation = new CustomAnnotation() {
      Identifier = pin.Id,
      Image = image,
      Title = pin.Label,
      Subtitle = pin.Address,
      Coordinate = new CLLocationCoordinate2D(pin.Location.Latitude,
                                              pin.Location.Longitude),
    };

    var nativeMap = (MauiMKMapView?)pin.Map?.Handler?.PlatformView;
    if (nativeMap is not null) {
      var customAnnotations =
          nativeMap.Annotations.OfType<CustomAnnotation>()
              .Where(x => x.Identifier == annotation.Identifier)
              .ToArray();
      nativeMap.RemoveAnnotations(customAnnotations);
          nativeMap.GetViewForAnnotation += GetViewForAnnotations;
          nativeMap.AddAnnotation(annotation);
    }
  }

  private static MKAnnotationView
  GetViewForAnnotations(MKMapView mapView, IMKAnnotation annotation) {
    MKAnnotationView? annotationView = null;

    if (annotation is CustomAnnotation customAnnotation) {
      annotationView = mapView.DequeueReusableAnnotation(
                           customAnnotation.Identifier.ToString()) ??
                       new MKAnnotationView(
                           annotation, customAnnotation.Identifier.ToString());
      annotationView.Image = customAnnotation.Image;
      annotationView.CanShowCallout = true;
    }

    return annotationView ?? new MKAnnotationView(annotation, null);
  }
}