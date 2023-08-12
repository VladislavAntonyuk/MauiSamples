
namespace MauiShellCustomization;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;
using Tizen.NUI;

internal class CustomShellHandler : ShellHandler
{
	protected override ShellView CreatePlatformView()
	{
		var view = base.CreatePlatformView();
		view.Margin = new Extents(20, 20, 20, 20);
		return view;
	}
}