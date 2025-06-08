namespace MauiIpCamera.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;

public partial class CameraViewModel : ObservableObject
{
	private const int Frequency = 100;
	private const int MaxConnectionsCount = 10;

	private readonly LocalHttpServer server;
	private PeriodicTimer? timer;

	public CameraViewModel(ILocalIpService localIpService)
	{
		const int port = 5555;
		var localIp = localIpService.GetLocalIpAddress();
		server = new LocalHttpServer(localIp, port, Frequency);
		IpAddressText = $"{localIp}:{port}/stream";

		AvailableResolutions = new ObservableCollection<Size>();
	}

	[ObservableProperty]
	public partial string IpAddressText { get; set; }

	public ObservableCollection<Size> AvailableResolutions { get; }

	[ObservableProperty]
	public partial Size SelectedResolution { get; set; } = new(800, 600);

	public async Task InitializeCameraAsync(CameraView cameraView)
	{
		var availableCameras = await cameraView.GetAvailableCameras(CancellationToken.None);
		cameraView.SelectedCamera = availableCameras.FirstOrDefault();

		AvailableResolutions.Clear();

		if (cameraView.SelectedCamera?.SupportedResolutions != null)
		{
			foreach (var resolution in cameraView.SelectedCamera.SupportedResolutions)
			{
				AvailableResolutions.Add(resolution);
			}
		}
	}

	public void OnMediaCaptured(Stream media)
	{
		server.SetStream(media);
	}

	[RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
	private async Task StartCamera(CameraView cameraView, CancellationToken cancellationToken)
	{
		DeviceDisplay.KeepScreenOn = true;
		_ = server.StartAsync(MaxConnectionsCount, cancellationToken);
		await CaptureLoopAsync(cameraView, cancellationToken);
		DeviceDisplay.KeepScreenOn = false;
	}

	private async Task CaptureLoopAsync(CameraView cameraView, CancellationToken cancellationToken)
	{
		cameraView.ImageCaptureResolution = SelectedResolution;
		await cameraView.StartCameraPreview(cancellationToken);

		timer = new PeriodicTimer(TimeSpan.FromMilliseconds(Frequency));
		while (await timer.WaitForNextTickAsync())
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

#if ANDROID
			await cameraView.SetExtensionMode(IsNight()
				                            ? AndroidX.Camera.Extensions.ExtensionMode.Night
				                            : AndroidX.Camera.Extensions.ExtensionMode.Auto);
#endif

			await cameraView.CaptureImage(CancellationToken.None);
		}

		cameraView.StopCameraPreview();
		server.Stop();
		timer.Dispose();
	}

	private static bool IsNight()
	{
		var currentTime = DateTime.Now;
		return currentTime.Hour is >= 18 or < 6;
	}
}