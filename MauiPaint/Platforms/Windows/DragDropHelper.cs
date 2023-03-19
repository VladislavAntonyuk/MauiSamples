using Microsoft.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using DataPackageOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation;
using DragEventArgs = Microsoft.UI.Xaml.DragEventArgs;

namespace MauiPaint;

using System.Text;

public static class DragDropHelper
{
	public static void RegisterDragDrop(UIElement element, Func<Stream, Task>? content)
	{
		element.AllowDrop = true;
		element.Drop += async (s, e) =>
		{
			if (e.DataView.Contains(StandardDataFormats.StorageItems))
			{
				var items = await e.DataView.GetStorageItemsAsync();
				foreach (var item in items)
				{
					if (item is StorageFile file)
					{
						if (content is not null)
						{
							var text = await FileIO.ReadTextAsync(file);
							var bytes = Encoding.Default.GetBytes(text);
							await content.Invoke(new MemoryStream(bytes));
						}
					}
				}
			}
		};
		element.DragOver += OnDragOver;
	}

	public static void UnRegisterDragDrop(UIElement element)
	{
		element.AllowDrop = false;
		element.DragOver -= OnDragOver;
	}

	private static async void OnDragOver(object sender, DragEventArgs e)
	{
		if (e.DataView.Contains(StandardDataFormats.StorageItems))
		{
			var deferral = e.GetDeferral();
			var extensions = new List<string> { ".json" };
			var isAllowed = false;
			var items = await e.DataView.GetStorageItemsAsync();
			foreach (var item in items)
			{
				if (item is StorageFile file && extensions.Contains(file.FileType))
				{
					isAllowed = true;
					break;
				}
			}

			e.AcceptedOperation = isAllowed ? DataPackageOperation.Copy : DataPackageOperation.None;
			deferral.Complete();
		}

		e.AcceptedOperation = DataPackageOperation.None;
	}
}