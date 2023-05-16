using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace MauiShellCustomization;

class CustomShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
{
	private readonly IShellContext shellContext;

	public CustomShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
	{
		this.shellContext = shellContext;
	}

	public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
	{
		base.SetAppearance(bottomView, appearance);
		var backgroundDrawable = new GradientDrawable();
		backgroundDrawable.SetShape(ShapeType.Rectangle);
		backgroundDrawable.SetCornerRadius(30);
		backgroundDrawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToPlatform());
		bottomView.SetBackground(backgroundDrawable);

		var layoutParams = bottomView.LayoutParameters;
		if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
		{
			var margin = 30;
			marginLayoutParams.BottomMargin = margin;
			marginLayoutParams.LeftMargin = margin;
			marginLayoutParams.RightMargin = margin;
			bottomView.LayoutParameters = layoutParams;
		}
	}

	protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
	{
		base.SetBackgroundColor(bottomView, color);
		bottomView.RootView?.SetBackgroundColor(shellContext.Shell.CurrentPage.BackgroundColor.ToPlatform());
	}
}