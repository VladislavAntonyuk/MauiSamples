namespace MauiMultiWindow;

using System.Threading.Tasks;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;

public static class WindowExtensions
{
	public static Task<T?> OpenModalWindow<T>(this Window parentWindow, ModalWindow<T> content)
	{
		ArgumentNullException.ThrowIfNull(parentWindow.Page?.Handler?.MauiContext);
		var xamlRoot = parentWindow.Page.ToPlatform(parentWindow.Page.Handler.MauiContext).XamlRoot;
		var dialog = new ContentDialog()
		{
			Content = content.ToPlatform(parentWindow.Page.Handler.MauiContext),
			XamlRoot = xamlRoot
		};

		var result = await dialog.ShowAsync();
		return result;
	}
}