using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace MauiShellCustomization;

class CustomShellHandler : ShellRenderer
{
	protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
	{
		return new CustomShellBottomNavViewAppearanceTracker(this, shellItem.CurrentItem);
	}

	protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
	{
		return new CustomShellToolbarAppearanceTracker(this);
	}
}