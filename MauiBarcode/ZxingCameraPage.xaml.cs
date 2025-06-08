namespace MauiBarcode;

using Camera.MAUI;
using Camera.MAUI.ZXing;
using Camera.MAUI.ZXingHelper;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CameraPosition = Camera.MAUI.CameraPosition;

public partial class ZxingCameraPage : ContentPage
{
	public ZxingCameraPage()
	{
		InitializeComponent();
		ZxingCameraView.BarCodeOptions = new BarcodeDecodeOptions
		{
			AutoRotate = true,
			PossibleFormats = [BarcodeFormat.QR_CODE],
		};
		ZxingCameraView.BarCodeDecoder = new ZXingBarcodeDecoder();
	}

	private void ZxingCameraView_CamerasLoaded(object? sender, EventArgs e)
	{
		if (ZxingCameraView.NumCamerasDetected == 0)
		{
			return;
		}

#if WINDOWS
		ZxingCameraView.Camera = ZxingCameraView.Cameras.FirstOrDefault();
#else
		ZxingCameraView.Camera = ZxingCameraView.Cameras.FirstOrDefault(x => x.Position != CameraPosition.Front);
#endif
	}

	private async void ZxingCameraViewOnBarcodeDetected(object sender, BarcodeEventArgs args)
	{
		foreach (var result in args.Result)
		{
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				await Toast.Make(result.Text, ToastDuration.Long).Show();
			});
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		ZxingCameraView.BarcodeDetected += ZxingCameraViewOnBarcodeDetected;
		ZxingCameraView.CamerasLoaded += ZxingCameraView_CamerasLoaded;
	}

	protected override void OnDisappearing()
	{
		ZxingCameraView.BarcodeDetected -= ZxingCameraViewOnBarcodeDetected;
		ZxingCameraView.CamerasLoaded -= ZxingCameraView_CamerasLoaded;

		base.OnDisappearing();
	}

	private async void StartButton_OnClicked(object? sender, EventArgs e)
	{
		await MainThread.InvokeOnMainThreadAsync(async () =>
		{
			await ZxingCameraView.StartCameraAsync();
		});
	}

	private async void StopButton_OnClicked(object? sender, EventArgs e)
	{
		await MainThread.InvokeOnMainThreadAsync(async () =>
		{
			await ZxingCameraView.StopCameraAsync();
		});
	}
}