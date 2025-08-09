namespace MauiIpCamera;

using Microsoft.Maui.Platform;
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
		await viewModel.InitializeCameraAsync(ToolkitCameraView);
		Loaded -= OnPageLoaded;
	}
}