namespace MauiBarcode;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnMlKitScannerClicked(object sender, EventArgs e)
	{
#if ANDROID
		using var mlkit = new MlKitBarcodeScanner();
		var barcode = await mlkit.ScanAsync();
		await MainThread.InvokeOnMainThreadAsync(async () =>
		{
			await Toast.Make(barcode is null ? "Error has occurred during barcode scanning" : barcode.RawValue, ToastDuration.Long).Show();
		});
#else
		await Toast.Make("This feature is only available on Android", ToastDuration.Long).Show();
#endif
	}
}