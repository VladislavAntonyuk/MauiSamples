using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiShellCustomization;

class CustomShellHandler : ShellRenderer
{
	protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
	{
		return new CustomShellTabBarAppearanceTracker();
	}

	protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
	{
		return new CustomShellToolbarAppearanceTracker(this, base.CreateNavBarAppearanceTracker());
	}
}