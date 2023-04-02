namespace MauiMultiWindow;

using System.Threading.Tasks;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;

public static class WindowExtensions
{
	public static async Task<T?> OpenModalWindowAsync<T>(this Window parentWindow, ModalWindow<T> content)
	{
		ArgumentNullException.ThrowIfNull(parentWindow.Page?.Handler?.MauiContext);
		var xamlRoot = parentWindow.Page.ToPlatform(parentWindow.Page.Handler.MauiContext).XamlRoot;
		var dialog = new ContentDialog()
		{
			Content = content.Content.ToPlatform(parentWindow.Page.Handler.MauiContext),
			XamlRoot = xamlRoot,
			PrimaryButtonText = content.SubmitContent,
			SecondaryButtonText = content.CancelContent
		};

		var result = await dialog.ShowAsync();
		return result switch
		{
			ContentDialogResult.Primary => await content.SubmitContentAction(),
			ContentDialogResult.Secondary => default,
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}