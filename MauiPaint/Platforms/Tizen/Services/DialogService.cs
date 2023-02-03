namespace MauiPaint.Platforms.Services;

using MauiPaint.Services;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

public class DialogService : IDialogService
{
	public async Task<bool> SaveFileDialog(Stream stream, string fileExtension, CancellationToken cancellationToken)
	{
		var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
		if (status == PermissionStatus.Granted)
		{
			var path = "/";
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}

			try
			{
				await WriteStream(stream, path, cancellationToken);

				return true;
			}
			catch
			{
				await Toast.Make("File is not stored").Show(cancellationToken);
				return false;
			}
		}

		await Toast.Make("Storage permission is not granted").Show(cancellationToken);
		return false;
	}

	public async Task<Stream> OpenFileDialog(CancellationToken cancellationToken)
	{
		var status = await Permissions.RequestAsync<Permissions.StorageRead>();
		if (status == PermissionStatus.Granted)
		{
			var path = "data.json";
			return File.Exists(path) ? new MemoryStream(await File.ReadAllBytesAsync(path, cancellationToken)) : Stream.Null;
		}

		await Toast.Make("Storage permission is not granted").Show(cancellationToken);
		return Stream.Null;
	}

	private static async Task WriteStream(Stream stream, string filePath, CancellationToken cancellationToken)
	{
		await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
		stream.Seek(0, SeekOrigin.Begin);
		await stream.CopyToAsync(fileStream, cancellationToken);
	}
}