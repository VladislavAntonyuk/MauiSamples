namespace MauiMaps;

using MapKit;
using Microsoft.Maui.Maps;
using UIKit;

public class CustomAnnotation : MKPointAnnotation
{
	public Guid Identifier { get; set; }
	public UIImage? Image { get; set; }
	public required IMapPin Pin { get; set; }
}