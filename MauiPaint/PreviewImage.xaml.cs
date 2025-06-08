namespace MauiPaint;

using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;

public partial class PreviewImage : Popup
{
	public StreamImageSource ImageSource { get; }

	public PreviewImage(StreamImageSource imageSource)
	{
#if WINDOWS
		var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
		WidthRequest = Math.Min(500, mainDisplayInfo.Width / mainDisplayInfo.Density);
		HeightRequest = Math.Min(500, mainDisplayInfo.Height / mainDisplayInfo.Density);
#endif

		ImageSource = imageSource;
		InitializeComponent();
		BindingContext = this;

#if MACCATALYST || WINDOWS
		Opened += (sender, args) =>
		{
			Preview.RegisterDrag(Handler.MauiContext, imageSource.Stream);
		};

		Closed += (sender, args) =>
		{
			Preview.UnRegisterDrag(Handler.MauiContext);
		};
#endif
	}
}