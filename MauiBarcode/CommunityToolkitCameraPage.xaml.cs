namespace MauiBarcode;

#if WINDOWS
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
#endif
using Camera.MAUI.ZXing;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Graphics.Platform;

public partial class CommunityToolkitCameraPage : ContentPage
{
	private readonly ZXingBarcodeDecoder barcodeReader = new ();

	public CommunityToolkitCameraPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		ToolkitCameraView.MediaCaptured += OnMediaCaptured;

		var cameras = await ToolkitCameraView.GetAvailableCameras(CancellationToken.None);
		ToolkitCameraView.SelectedCamera = cameras.FirstOrDefault(x => x.Position != CommunityToolkit.Maui.Core.Primitives.CameraPosition.Front);

		await Task.Delay(1000);
		await ToolkitCameraView.StartCameraPreview(CancellationToken.None);

		PeriodicTimer timer = new(TimeSpan.FromMilliseconds(3000));
		while (await timer.WaitForNextTickAsync())
		{
			await ToolkitCameraView.CaptureImage(CancellationToken.None);
		}
	}

	protected override void OnDisappearing()
	{
		ToolkitCameraView.MediaCaptured -= OnMediaCaptured;
		base.OnDisappearing();
	}

	private async void OnMediaCaptured(object? sender, MediaCapturedEventArgs e)
	{
		try
		{
			var image = PlatformImage.FromStream(e.Media);
#if ANDROID
			var results = barcodeReader.Decode(image.AsBitmap());
#elif IOS || MACCATALYST
			var results = barcodeReader.Decode(image.AsUIImage());
#elif WINDOWS
			var softwareBitmap = SoftwareBitmap.CreateCopyFromBuffer(
				image.AsBytes().AsBuffer(),
				BitmapPixelFormat.Rgba16,
				(int)image.Width,
				(int)image.Height);
			var results = barcodeReader.Decode(softwareBitmap);
#else
			var results = new List<BarcodeResult>();
#endif
			foreach (var result in results ?? [])
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await Toast.Make(result.Text, ToastDuration.Long).Show();
				});
			}
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception);
		}
	}
}