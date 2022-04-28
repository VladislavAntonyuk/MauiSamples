namespace MauiPaint.Platforms.Services;

using Foundation;
using MauiPaint.Services;
using UIKit;

public class DialogService : IDialogService
{
	public async Task<bool> SaveFileDialog(Stream stream, string fileExtension, CancellationToken cancellationToken)
	{
		var fileManager = NSFileManager.DefaultManager;
		var fileUrl = fileManager.GetTemporaryDirectory().Append($"temp{fileExtension}", false);
		await WriteStream(stream, fileUrl.Path, cancellationToken);
		var documentPickerViewController = new UIDocumentPickerViewController(new[] { fileUrl });
		var currentViewController = GetCurrentUIController();
		var taskCompetedSource = new TaskCompletionSource<bool>();
		documentPickerViewController.DidPickDocumentAtUrls += (s, e) =>
		{
			taskCompetedSource.SetResult(true);
		};
		documentPickerViewController.WasCancelled += (s, e) =>
		{
			taskCompetedSource.SetResult(false);
		};
		currentViewController?.PresentViewController(documentPickerViewController, true, null);
		return await taskCompetedSource.Task;
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

	private static async Task WriteStream(Stream stream, string filePath, CancellationToken cancellationToken)
	{
		await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
		stream.Seek(0, SeekOrigin.Begin);
		await stream.CopyToAsync(fileStream, cancellationToken);
	}

	private static UIViewController? GetCurrentUIController()
	{
		var viewController = UIApplication.SharedApplication.ConnectedScenes.ToArray()
										  .Where(x => x.ActivationState == UISceneActivationState.ForegroundActive)
										  .Select(x => x as UIWindowScene)
										  .FirstOrDefault()?
										  .Windows.FirstOrDefault(x => x.IsKeyWindow)?.RootViewController;


		while (viewController?.PresentedViewController != null)
		{
			viewController = viewController.PresentedViewController;
		}

		return viewController;
	}
}