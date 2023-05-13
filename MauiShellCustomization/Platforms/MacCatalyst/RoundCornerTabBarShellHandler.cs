using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiShellCustomization;

class RoundCornerTabBarShellHandler : ShellRenderer
{
	protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
	{
		return new RoundCornerBottomNavViewAppearanceTracker();
	}

	protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
	{
		return new RoundCornerShellToolbarAppearanceTracker(this, base.CreateNavBarAppearanceTracker());
	}
}