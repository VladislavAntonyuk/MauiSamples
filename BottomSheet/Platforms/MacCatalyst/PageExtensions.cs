namespace BottomSheet;

using Microsoft.Maui.Platform;
using UIKit;

public static partial class PageExtensions
{
	public static void ShowBottomSheetPlatform(this Page page, IView bottomSheetContent)
	{
		var mauiContext = page.Handler?.MauiContext ?? throw new Exception("MauiContext is null");
		var viewController = page.ToUIViewController(mauiContext);
		var viewControllerToPresent = bottomSheetContent.ToUIViewController(mauiContext);

		var sheet = viewControllerToPresent.SheetPresentationController;
		if (sheet is not null)
		{
			sheet.Detents = new[]
			{
				UISheetPresentationControllerDetent.CreateMediumDetent(),
				UISheetPresentationControllerDetent.CreateLargeDetent(),
			};
			sheet.LargestUndimmedDetentIdentifier = UISheetPresentationControllerDetentIdentifier.Medium;
			sheet.PrefersScrollingExpandsWhenScrolledToEdge = false;
			sheet.PrefersEdgeAttachedInCompactHeight = true;
			sheet.WidthFollowsPreferredContentSizeWhenEdgeAttached = true;
		}

		viewController.PresentViewController(viewControllerToPresent, animated: true, null);
	}
}