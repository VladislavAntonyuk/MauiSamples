using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;

namespace MauiShellCustomization;

class CustomShellHandler : ShellHandler
{
	protected override ShellView CreatePlatformView()
	{
		var view = base.CreatePlatformView();
		view.Margin = new Tizen.NUI.Extents(20, 20, 20, 20);
		return view;
	}
}