namespace MauiIpCamera;

using CommunityToolkit.Maui.Core;
using ViewModels;

public partial class CommunityToolkitCameraPage
{
	private readonly CameraViewModel viewModel;

	public CommunityToolkitCameraPage(CameraViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		var cameraRequest = await Permissions.RequestAsync<Permissions.Camera>();
		var microphoneRequest = await Permissions.RequestAsync<Permissions.Microphone>();
		if (cameraRequest != PermissionStatus.Granted || microphoneRequest != PermissionStatus.Granted)
		{
			await DisplayAlert("Permission Denied", "Camera permission is required to use this feature.", "OK");
			return;
		}

		await viewModel.InitializeCameraAsync(ToolkitCameraView);
		ToolkitCameraView.MediaCaptured += ToolkitCameraView_MediaCaptured;
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		ToolkitCameraView.MediaCaptured -= ToolkitCameraView_MediaCaptured;
		viewModel.StartCameraCancelCommand.Execute(null);
		base.OnNavigatedFrom(args);
	}

	private void ToolkitCameraView_MediaCaptured(object? sender, MediaCapturedEventArgs e)
	{
		viewModel.OnMediaCaptured(e.Media);
	}
}