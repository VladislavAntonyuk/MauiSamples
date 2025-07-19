namespace MauiIpCamera;

using ViewModels;

public partial class MainPage
{
	private readonly CameraViewModel viewModel;

	public MainPage(CameraViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
		Loaded += OnPageLoaded;
	}

	private async void OnPageLoaded(object? sender, EventArgs e)
	{
		var cameraRequest = await Permissions.RequestAsync<Permissions.Camera>();
		var microphoneRequest = await Permissions.RequestAsync<Permissions.Microphone>();
		if (cameraRequest != PermissionStatus.Granted || microphoneRequest != PermissionStatus.Granted)
		{
			await DisplayAlert("Permission Denied", "Camera permission is required to use this feature.", "OK");
			return;
		}

		await viewModel.InitializeCameraAsync(ToolkitCameraView);
		Loaded -= OnPageLoaded;
	}
}