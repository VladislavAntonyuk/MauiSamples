namespace MauiMaps;

using MapKit;
using UIKit;

public class CustomAnnotation : MKPointAnnotation
{
    public Guid Identifier {
        get;
        set;
    }
    public UIImage? Image {
        get;
        set;
    }
}