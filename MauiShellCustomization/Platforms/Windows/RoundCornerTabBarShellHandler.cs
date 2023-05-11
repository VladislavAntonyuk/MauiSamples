using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;

namespace MauiShellCustomization;

class RoundCornerTabBarShellHandler : ShellHandler
{
	protected override ShellView CreatePlatformView()
	{
		var view = base.CreatePlatformView();
		view.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
		return view;
	}
}