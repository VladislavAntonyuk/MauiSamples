namespace MauiMaps;

using Microsoft.Maui.Maps;

public partial class MainPage : ContentPage {
  public MainPage() {
    InitializeComponent();
    var customPin1 = new CustomPin() {
      Label = "label", Location = new Location(10, 10), Address = "Address",
      ImageSource = ImageSource.FromUri(new Uri("https://picsum.photos/50")),
      Map = MyMap
    };
    var customPin2 = new CustomPin() {
      Label = "label2", Location = new Location(11, 11), Address = "Address2",
      ImageSource = ImageSource.FromUri(new Uri("https://picsum.photos/60")),
      Map = MyMap
    };
    MyMap.Pins.Add(customPin1);
    MyMap.Pins.Add(customPin2);
    MyMap.MoveToRegion(new MapSpan(new Location(10, 10), 10, 10));
  }
}
