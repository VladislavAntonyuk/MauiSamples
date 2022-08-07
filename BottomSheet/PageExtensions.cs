namespace BottomSheet;

using System.Diagnostics;

public static partial class PageExtensions
{
	[Conditional("ANDROID"), Conditional("IOS"), Conditional("MACCATALYST")]
	public static void ShowBottomSheet(this Page page, IView bottomSheetContent)
	{
		ShowBottomSheetPlatform(page, bottomSheetContent);
	}
}