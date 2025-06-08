namespace MauiPaint;

using CommunityToolkit.Maui.Views;

public partial class Help : Popup
{
	public Help()
	{
		InitializeComponent();
		var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
		WidthRequest = Math.Min(500, mainDisplayInfo.Width / mainDisplayInfo.Density);
		HeightRequest = Math.Min(500, mainDisplayInfo.Height / mainDisplayInfo.Density);
	}
}