namespace BottomSheet;

using Android.App;
using Android.Views;
using Google.Android.Material.BottomSheet;
using Microsoft.Maui.Platform;

public static partial class PageExtensions
{
	public static void ShowBottomSheet(this Page page, IView bottomSheetContent, bool dimDismiss)
	{
		var bottomSheetDialog = new BottomSheetDialog(Platform.CurrentActivity?.Window?.DecorView.FindViewById(Android.Resource.Id.Content)?.Context ?? throw new InvalidOperationException("Context is null"));
		bottomSheetDialog.SetContentView(bottomSheetContent.ToPlatform(page.Handler?.MauiContext ?? throw new Exception("MauiContext is null")));
		bottomSheetDialog.Show();
	}
}