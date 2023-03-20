namespace MauiMultiWindow;

using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;

public static class WindowExtensions
{
	public static async Task OpenModalWindow(this Window parentWindow, IView content)
	{
		ArgumentNullException.ThrowIfNull(parentWindow.Page?.Handler?.MauiContext);
		var xamlRoot = parentWindow.Page.ToPlatform(parentWindow.Page.Handler.MauiContext).XamlRoot;
		var dialog = new ContentDialog()
		{
			Content = content.ToPlatform(parentWindow.Page.Handler.MauiContext),
			XamlRoot = xamlRoot
		};

		var result = await dialog.ShowAsync();
	}
}