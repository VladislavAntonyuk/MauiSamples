namespace MauiMaps;

using MapKit;
using Microsoft.Maui.Maps;
using UIKit;

public class CustomAnnotation : MKPointAnnotation
{
	public Guid Identifier { get; init; }
	public UIImage? Image { get; init; }
	public required IMapPin Pin { get; init; }
}