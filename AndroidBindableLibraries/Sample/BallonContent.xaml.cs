namespace Sample;

using Android.Widget;

public partial class BallonContent : ContentView
{
	public BallonContent()
	{
		InitializeComponent();
	}

	private void Button_OnClicked(object? sender, EventArgs e)
	{
		Toast.MakeText(Platform.AppContext, "ButtonClicked", ToastLength.Long).Show();
	}
}