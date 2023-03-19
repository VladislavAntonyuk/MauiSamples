using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Microsoft.UI.Xaml;
using DataPackageOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation;
using DragEventArgs = Microsoft.UI.Xaml.DragEventArgs;
using Application = Microsoft.Maui.Controls.Application;
using System;
using System.IO;

namespace MauiPaint;


public static class DragDropHelper
{
	public static void RegisterDragDrop(UIElement element, Func<Stream, Task>? content)
	{
		element.AllowDrop = true;
		element.Drop += OnDrop;
		element.DragOver += OnDragOver;
	}

	public static void UnRegisterDragDrop(UIElement element)
	{
		element.AllowDrop = false;
		element.Drop -= OnDrop;
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

	private static async void OnDrop(object sender, DragEventArgs e)
	{
		if (e.DataView.Contains(StandardDataFormats.StorageItems))
		{
			var items = await e.DataView.GetStorageItemsAsync();
			foreach (var item in items)
			{
				if (item is StorageFile file)
				{
					if (Content is not null)
					{
						var text = await FileIO.ReadTextAsync(file);
						var bytes = await File.ReadAllBytesAsync(nsData.Path);
						await Content.Invoke(new MemoryStream(bytes));
					}
				}
			}
		}
	}
}