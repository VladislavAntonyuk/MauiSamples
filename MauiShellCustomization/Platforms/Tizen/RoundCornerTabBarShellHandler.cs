using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiShellCustomization;

class RoundCornerTabBarShellHandler : ShellRenderer
{
	protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
	{
		return new RoundCornerBottomNavViewAppearanceTracker();
	}
}