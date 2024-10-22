namespace MauiBarcode;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Graphics.Platform;
using ZXing;
using ZXing.Common;

public partial class CommunityToolkitCameraPage : ContentPage
{
	private readonly IBarcodeReaderGeneric barcodeReader = new BarcodeReaderGeneric()
	{
		AutoRotate = true,
		Options = new DecodingOptions()
		{
			PossibleFormats = [BarcodeFormat.QR_CODE],
			TryHarder = true,
			TryInverted = true,
			PureBarcode = false
		}
	};
	PeriodicTimer timer = new(TimeSpan.FromMilliseconds(3000));

	public CommunityToolkitCameraPage()
	{
		InitializeComponent();
	}

#if !TIZEN
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		ToolkitCameraView.MediaCaptured += OnMediaCaptured;

		var cameras = await ToolkitCameraView.GetAvailableCameras(CancellationToken.None);
		ToolkitCameraView.SelectedCamera = cameras.FirstOrDefault(x => x.Position != CommunityToolkit.Maui.Core.Primitives.CameraPosition.Front);
	}

	protected override void OnDisappearing()
	{
		ToolkitCameraView.MediaCaptured -= OnMediaCaptured;
		base.OnDisappearing();
	}

	private async void OnMediaCaptured(object? sender, MediaCapturedEventArgs e)
	{
		var image = (PlatformImage)PlatformImage.FromStream(e.Media);
		var result = barcodeReader.Decode(new ImageLuminanceSource(image));
		if (result is null)
		{
			return;
		}

		await MainThread.InvokeOnMainThreadAsync(async () =>
		{
			await Toast.Make(result.Text, ToastDuration.Long).Show();
		});
	}
#endif

	private async void StartButton_OnClicked(object? sender, EventArgs e)
	{
		timer = new(TimeSpan.FromMilliseconds(3000));
#if !TIZEN
		await ToolkitCameraView.StartCameraPreview(CancellationToken.None);
		while (await timer.WaitForNextTickAsync())
		{
			await ToolkitCameraView.CaptureImage(CancellationToken.None);
		}
#endif
	}

	private void StopButton_OnClicked(object? sender, EventArgs e)
	{
		timer.Dispose();
#if !TIZEN
		ToolkitCameraView.StopCameraPreview();
#endif
	}
}