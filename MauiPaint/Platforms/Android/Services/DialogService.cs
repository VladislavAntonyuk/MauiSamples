namespace MauiPaint.Platforms.Services;

using Android.Services;
using MauiPaint.Services;

public class DialogService : IDialogService
{
	public async Task<bool> SaveFileDialog(Stream stream, string fileExtension, CancellationToken cancellationToken)
	{
		var dialog = new FileDialog(Platform.CurrentActivity, FileDialog.FileSelectionMode.FileSave, fileExtension);
		var path = await dialog.GetFileOrDirectoryAsync(GetExternalDirectory());
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		
		await WriteStream(stream, path, cancellationToken);

		return true;
	}

	public async Task<Stream> OpenFileDialog(CancellationToken cancellationToken)
	{
		var dialog = new FileDialog(Platform.CurrentActivity, FileDialog.FileSelectionMode.FileOpen, ".json");
		var path = await dialog.GetFileOrDirectoryAsync(GetExternalDirectory());
		return string.IsNullOrEmpty(path) ? Stream.Null : new MemoryStream(await File.ReadAllBytesAsync(path, cancellationToken));
	}

	private static async Task WriteStream(Stream stream, string filePath, CancellationToken cancellationToken)
	{
		await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
		stream.Seek(0, SeekOrigin.Begin);
		await stream.CopyToAsync(fileStream, cancellationToken);
	}

	string? GetExternalDirectory()
	{
		return Platform.CurrentActivity?.GetExternalFilesDir(null)
		               ?.ParentFile?.ParentFile?.ParentFile?.ParentFile?.AbsolutePath;
	}
}