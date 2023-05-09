using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;

namespace MauiShellCustomization;

class RoundCornerTabBarShellHandler : ShellHandler
{
	protected override ShellView CreatePlatformView()
	{
		var view = base.CreatePlatformView();
		//view.Margin = new Microsoft.UI.Xaml.Thickness(20, 20, 20, 20);
		return view;
	}
}