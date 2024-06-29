namespace MauiBarcode;

using Xamarin.Google.MLKit.Vision.Barcode.Common;
using Xamarin.Google.MLKit.Vision.CodeScanner;

public class MlKitBarcodeScanner : IDisposable
{
	private readonly IGmsBarcodeScanner barcodeScanner = GmsBarcodeScanning.GetClient(
		Platform.AppContext,
		new GmsBarcodeScannerOptions.Builder()
			.AllowManualInput()
			.EnableAutoZoom()
			.SetBarcodeFormats(Barcode.FormatAllFormats)
			.Build());

	public async Task<Barcode?> ScanAsync()
	{
		var taskCompletionSource = new TaskCompletionSource<Barcode?>();
		var barcodeResultListener = new OnBarcodeResultListener(taskCompletionSource);
		using var task = barcodeScanner.StartScan()
					   .AddOnCompleteListener(barcodeResultListener);
		return await taskCompletionSource.Task;
	}

	public void Dispose()
	{
		barcodeScanner.Dispose();
	}
}
