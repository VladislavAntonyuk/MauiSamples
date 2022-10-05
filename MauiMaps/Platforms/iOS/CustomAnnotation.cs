namespace MauiMaps;

using CoreLocation;
using MapKit;
using UIKit;

public class CustomAnnotation : MKAnnotation
{
	public CustomAnnotation(string title, CLLocationCoordinate2D coord, UIImage image)
	{
		Title = title;
		Coordinate = coord;
		Image = image;
	}

	public override string Title { get; }

	public override CLLocationCoordinate2D Coordinate { get; }
	public UIImage Image { get; }
}