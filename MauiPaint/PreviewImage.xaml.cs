namespace MauiPaint;

using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;

public partial class PreviewImage : Popup
{
	public StreamImageSource ImageSource { get; }

	public PreviewImage(StreamImageSource imageSource)
	{
		ImageSource = imageSource;
		InitializeComponent();
		BindingContext = this;
		var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
		Size = new Size(Math.Min(500, mainDisplayInfo.Width / mainDisplayInfo.Density), Math.Min(500, mainDisplayInfo.Height / mainDisplayInfo.Density));

#if MACCATALYST || WINDOWS
		Opened += (sender, args) =>
		{
			if (Handler?.MauiContext != null)
			{
				var uiElement = Preview.ToPlatform(Handler.MauiContext);
				DragDropHelper.RegisterDrag(uiElement, imageSource.Stream);
			}
		};

		Closed += (sender, args) =>
		{
			if (Handler?.MauiContext != null)
			{
				var uiElement = Preview.ToPlatform(Handler.MauiContext);
				DragDropHelper.UnRegisterDrag(uiElement);
			}
		};
#endif
	}
}