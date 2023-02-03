namespace BottomSheet;

using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

public static partial class PageExtensions
{
	public static Popup ShowBottomSheet(this Microsoft.Maui.Controls.Page page, IView bottomSheetContent, bool dimDismiss)
	{
		var mauiContext = page.Handler?.MauiContext ?? throw new Exception("MauiContext is null");
		var content = bottomSheetContent.ToPlatform(mauiContext);
		if (content is ScrollViewer scrollViewer)
		{
			page.Window.SizeChanged += delegate
			{
				scrollViewer.Height = page.Window.Height;
				scrollViewer.Width = page.Window.Width;
			};
			scrollViewer.Height = page.Window.Height;
			scrollViewer.Width = page.Window.Width;
		}

		var dialog = new Popup
		{
			XamlRoot = page.ToPlatform(mauiContext).XamlRoot,
			Child = content,
			LightDismissOverlayMode = dimDismiss ? LightDismissOverlayMode.On : LightDismissOverlayMode.Off,
			IsLightDismissEnabled = dimDismiss,
			IsOpen = true
		};

		return dialog;
	}

	public static void CloseBottomSheet(this Popup bottomSheet)
	{
		bottomSheet.IsOpen = false;
	}
}