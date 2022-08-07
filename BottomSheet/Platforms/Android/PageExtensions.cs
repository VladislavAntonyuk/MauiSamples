namespace BottomSheet;

using Android.App;
using Android.Views;
using Google.Android.Material.BottomSheet;
using Microsoft.Maui.Platform;

public static partial class PageExtensions
{
	public static void ShowBottomSheetPlatform(this Page page, IView bottomSheetContent)
	{
		var bottomSheetDialog = new BottomSheetDialog(Platform.CurrentActivity?.Window?.DecorView.FindViewById(Android.Resource.Id.Content)?.RootView?.Context);
		bottomSheetDialog.SetContentView(bottomSheetContent.ToPlatform(page.Handler?.MauiContext ?? throw new Exception("MauiContext is null")));
		bottomSheetDialog.Show();
	}
}