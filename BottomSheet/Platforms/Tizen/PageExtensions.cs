namespace BottomSheet;

using Microsoft.Maui.Platform;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.NUI;
using Color = Tizen.NUI.Color;

public static partial class PageExtensions
{
	public static Popup ShowBottomSheet(this Page page, IView bottomSheetContent, bool dimDismiss)
	{
		var mauiContext = page.Handler?.MauiContext ?? throw new Exception("MauiContext is null");
		var content = bottomSheetContent.ToPlatform(mauiContext);
		var contentSize = bottomSheetContent.Measure(double.PositiveInfinity, double.PositiveInfinity).ToPixel();
		var popup = new Popup
		{
			Content = content,
			Position = Position.ParentOriginBottomCenter,
			BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f),
			Layout = new LinearLayout
			{
				LinearOrientation = LinearLayout.Orientation.Vertical,
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Center,
			}
		};
		popup.Content.SizeWidth = (float)contentSize.Width;
		if (dimDismiss)
		{
			popup.OutsideClicked += delegate
			{
				popup.Close();
			};
		}

		popup.Open();
		return popup;
	}

	public static void CloseBottomSheet(this Popup bottomSheet)
	{
		bottomSheet.Close();
	}
}