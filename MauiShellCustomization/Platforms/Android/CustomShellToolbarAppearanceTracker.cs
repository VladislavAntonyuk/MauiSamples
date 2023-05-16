namespace MauiShellCustomization;

using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

class CustomShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
{
	public CustomShellToolbarAppearanceTracker(IShellContext shellContext) : base(shellContext)
	{
	}

	public override void SetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
	{
		base.SetAppearance(toolbar, toolbarTracker, appearance);
		var backgroundDrawable = new GradientDrawable();
		backgroundDrawable.SetShape(ShapeType.Rectangle);
		backgroundDrawable.SetCornerRadius(30);
		backgroundDrawable.SetColor(appearance.BackgroundColor.ToPlatform());
		toolbar.SetBackground(backgroundDrawable);

		var layoutParams = toolbar.LayoutParameters;
		if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
		{
			var margin = 30;
			marginLayoutParams.TopMargin = margin;
			marginLayoutParams.BottomMargin = margin;
			marginLayoutParams.LeftMargin = margin;
			marginLayoutParams.RightMargin = margin;
			toolbar.LayoutParameters = layoutParams;
		}
	}
}