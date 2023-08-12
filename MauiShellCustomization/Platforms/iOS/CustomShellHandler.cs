using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiShellCustomization;

internal class CustomShellHandler : ShellRenderer
{
	protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
	{
		return new CustomShellTabBarAppearanceTracker();
	}

	protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
	{
		return new CustomShellNavBarAppearanceTracker(this, base.CreateNavBarAppearanceTracker());
	}

	protected override IShellItemRenderer CreateShellItemRenderer(ShellItem item)
	{
		return new CustomShellItemRenderer(this)
		{
			ShellItem = item
		};
	}
}