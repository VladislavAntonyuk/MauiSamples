using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiShellCustomization;

class RoundCornerBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
{
    private readonly IShellContext shellContext;

    public RoundCornerBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
    {
        this.shellContext = shellContext;
    }

    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);
        var backgroundDrawable = new GradientDrawable();
        backgroundDrawable.SetShape(ShapeType.Rectangle);
        backgroundDrawable.SetCornerRadius(30);
        backgroundDrawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToAndroid());
        bottomView.SetBackground(backgroundDrawable);

        var layoutParams = bottomView.LayoutParameters;
        if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
        {
            marginLayoutParams.BottomMargin = 30;
            marginLayoutParams.LeftMargin = 30;
            marginLayoutParams.RightMargin = 30;
            bottomView.LayoutParameters = layoutParams;
        }
    }

    protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
    {
        base.SetBackgroundColor(bottomView, color);
        bottomView.RootView?.SetBackgroundColor(shellContext.Shell.CurrentPage.BackgroundColor.ToAndroid());
    }
}