namespace MauiPaint.Platforms.Services;

using System.Diagnostics;
using Windows.Storage.Pickers;
using MauiPaint.Services;

public class DialogService : IDialogService
{
	public async Task<bool> SaveFileDialog(Stream stream, string fileExtension, CancellationToken cancellationToken)
	{
		var savePicker = new FileSavePicker
		{
			SuggestedStartLocation = PickerLocationId.DocumentsLibrary
		};
		WinRT.Interop.InitializeWithWindow.Initialize(savePicker, Process.GetCurrentProcess().MainWindowHandle);
		savePicker.FileTypeChoices.Add("Paint file", new List<string> { fileExtension });
		savePicker.SuggestedFileName = "Project 1";
		var file = await savePicker.PickSaveFileAsync();
		if (string.IsNullOrEmpty(file?.Path))
		{
			return false;
		}

		await WriteStream(stream, file.Path, cancellationToken);
		return true;

	}

	public async Task<Stream> OpenFileDialog(CancellationToken cancellationToken)
	{
		var fileResult = await FilePicker.PickAsync(PickOptions.Default);
		if (fileResult is null)
		{
			return Stream.Null;
		}

		return await fileResult.OpenReadAsync();
	}

	static async Task WriteStream(Stream stream, string filePath, CancellationToken cancellationToken)
	{
		await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
		stream.Seek(0, SeekOrigin.Begin);
		await stream.CopyToAsync(fileStream, cancellationToken);
	}
}