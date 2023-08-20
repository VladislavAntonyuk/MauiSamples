using Microsoft.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using DataPackageOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation;
using DragEventArgs = Microsoft.UI.Xaml.DragEventArgs;

namespace MauiPaint;

using System.Diagnostics;
using System.Text;
using Windows.Foundation;
using DragStartingEventArgs = Microsoft.UI.Xaml.DragStartingEventArgs;

public static class DragDropHelper
{
	private static readonly Dictionary<UIElement, TypedEventHandler<UIElement, DragStartingEventArgs>> DragStartingEventHandlers = new();
	private static readonly Dictionary<UIElement, DragEventHandler> DragEventHandlers = new();

	public static void RegisterDrag(UIElement element, Func<CancellationToken, Task<Stream>> content)
	{
		element.CanDrag = true;

		async void DragStartingHandler(UIElement s, DragStartingEventArgs e)
		{
			var stream = await content.Invoke(CancellationToken.None);
			var storageFile = await CreateStorageFile(stream);
			e.Data.SetStorageItems(new List<IStorageItem>()
			{
				storageFile
			});
		}

		DragStartingEventHandlers[element] = DragStartingHandler;
		element.DragStarting += DragStartingHandler;
	}

	public static void RegisterDrop(UIElement element, Func<Stream, Task>? content)
	{
		element.AllowDrop = true;
		async void DropHandler(object s, DragEventArgs e)
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
		}

		element.Drop += DropHandler;
		DragEventHandlers[element] = DropHandler;
		element.DragOver += OnDragOver;
	}

	public static void UnRegisterDrag(UIElement element)
	{
		element.CanDrag = false;
		if (DragStartingEventHandlers.TryGetValue(element, out var dragStartingEventHandler))
		{
			element.DragStarting -= dragStartingEventHandler;
			DragStartingEventHandlers.Remove(element);
		}
	}

	public static void UnRegisterDrop(UIElement element)
	{
		element.AllowDrop = false;
		if (DragEventHandlers.TryGetValue(element, out var dragEventHandler))
		{
			element.Drop -= dragEventHandler;
			DragEventHandlers.Remove(element);
		}

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

	private static IAsyncOperation<StorageFile> CreateStorageFile(Stream imageStream)
	{
		var filename = "SampleImage.jpg";
		return StorageFile.CreateStreamedFileAsync(filename, async stream => await StreamDataRequestedAsync(stream, imageStream), null);
	}

	private static async Task StreamDataRequestedAsync(StreamedFileDataRequest request, Stream imageDataStream)
	{
		try
		{
			await using (var outputStream = request.AsStreamForWrite())
			{
				await imageDataStream.CopyToAsync(outputStream);
				await outputStream.FlushAsync();
			}
			request.Dispose();
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex.Message);
			request.FailAndClose(StreamedFileFailureMode.Incomplete);
		}
	}
}