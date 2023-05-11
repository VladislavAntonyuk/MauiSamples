namespace MauiShellCustomization;

using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;

class RoundCornerShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
{
	public RoundCornerShellToolbarAppearanceTracker(IShellContext shellContext) : base(shellContext)
	{
	}

	public override void SetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
	{
		base.SetAppearance(toolbar, toolbarTracker, appearance);
		var backgroundDrawable = new GradientDrawable();
		backgroundDrawable.SetShape(ShapeType.Rectangle);
		backgroundDrawable.SetCornerRadius(30);
		backgroundDrawable.SetColor(appearance.BackgroundColor.ToAndroid());
		toolbar.SetBackground(backgroundDrawable);

		var layoutParams = toolbar.LayoutParameters;
		if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
		{
			marginLayoutParams.TopMargin = 30;
			marginLayoutParams.BottomMargin = 30;
			marginLayoutParams.LeftMargin = 30;
			marginLayoutParams.RightMargin = 30;
			toolbar.LayoutParameters = layoutParams;
		}
	}
}