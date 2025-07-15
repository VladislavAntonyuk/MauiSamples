namespace MauiIpCamera.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;

public partial class CameraViewModel : ObservableObject
{
	private const int Port = 5555;
	private const int Frequency = 100;
	private const int VideoDurationMs = 10000;
	private const int MaxConnectionsCount = 10;
	private readonly string ipAddress;

	private readonly LocalHttpServer server;

	public CameraViewModel(ILocalIpService localIpService)
	{
		var localIp = localIpService.GetLocalIpAddress();
		server = new LocalHttpServer(localIp, Port, Frequency);
		IpAddressText = ipAddress = $"{localIp}:{Port}";

		AvailableResolutions = new ObservableCollection<Size>();
		RecordingsFolder = Preferences.Get(nameof(RecordingsFolder), null);
		if (!string.IsNullOrWhiteSpace(RecordingsFolder))
		{
			SaveRecordingToFileStorage = true;
		}
	}

	[ObservableProperty]
	public partial bool SaveRecordingToFileStorage { get; set; }

	[ObservableProperty]
	public partial string? RecordingsFolder { get; set; }

	[ObservableProperty]
	public partial bool IsPowerSavingModeEnabled { get; set; }

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

	[RelayCommand]
	void EnablePowerSavingMode()
	{
		IsPowerSavingModeEnabled = true;
	}

	[RelayCommand]
	void DisablePowerSavingMode()
	{
		IsPowerSavingModeEnabled = false;
	}

	[RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
	private async Task StartMjpegStream(CameraView cameraView, CancellationToken cancellationToken)
	{
		IpAddressText = $"http://{ipAddress}/mjpeg";
		DeviceDisplay.KeepScreenOn = true;
		_ = server.StartAsync(MaxConnectionsCount, cancellationToken);
		await CaptureMjpegAsync(cameraView, cancellationToken);
		DeviceDisplay.KeepScreenOn = false;
	}

	[RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
	private async Task StartVideoStream(CameraView cameraView, CancellationToken cancellationToken)
	{
		IpAddressText = $"http://{ipAddress}/player";
		DeviceDisplay.KeepScreenOn = true;
		_ = server.StartAsync(MaxConnectionsCount, cancellationToken);
		await CaptureVideoAsync(cameraView, cancellationToken);
		DeviceDisplay.KeepScreenOn = false;
	}

	private async Task CaptureMjpegAsync(CameraView cameraView, CancellationToken cancellationToken)
	{
#if ANDROID
		var mode = IsNight() ? AndroidX.Camera.Extensions.ExtensionMode.Night : AndroidX.Camera.Extensions.ExtensionMode.Auto;
		await cameraView.SetExtensionMode(mode);
#endif
		cameraView.ImageCaptureResolution = SelectedResolution;
		await cameraView.StartCameraPreview(cancellationToken);

		while (!cancellationToken.IsCancellationRequested)
		{
#if ANDROID
			var newMode = IsNight() ? AndroidX.Camera.Extensions.ExtensionMode.Night : AndroidX.Camera.Extensions.ExtensionMode.Auto;
			if (newMode != mode)
			{
				await cameraView.SetExtensionMode(newMode);
				mode = newMode;
			}
#endif

			await using var stream = await cameraView.CaptureImage(CancellationToken.None);
			server.SetMjpegStream(stream);
			await Task.Delay(Frequency, CancellationToken.None);
		}

		cameraView.StopCameraPreview();
		server.Stop();
	}

	private async Task CaptureVideoAsync(CameraView cameraView, CancellationToken cancellationToken)
	{
#if ANDROID
		var mode = IsNight() ? AndroidX.Camera.Extensions.ExtensionMode.Night : AndroidX.Camera.Extensions.ExtensionMode.Auto;
		await cameraView.SetExtensionMode(mode);
#endif
		cameraView.ImageCaptureResolution = SelectedResolution;
		await cameraView.StartCameraPreview(cancellationToken);

		while (!cancellationToken.IsCancellationRequested)
		{
#if ANDROID
			var newMode = IsNight() ? AndroidX.Camera.Extensions.ExtensionMode.Night : AndroidX.Camera.Extensions.ExtensionMode.Auto;
			if (newMode != mode)
			{
				await cameraView.SetExtensionMode(newMode);
				mode = newMode;
			}
#endif
			using var stream = new MemoryStream();
			await cameraView.StartVideoRecording(stream, CancellationToken.None);
			await Task.Delay(VideoDurationMs, CancellationToken.None);
			await cameraView.StopVideoRecording(CancellationToken.None);
			server.SetVideoStream(stream);
			await SaveStreamToFileSystem(stream, $"recording-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}.mp4");
		}

		cameraView.StopCameraPreview();
		server.Stop();
	}

	private static bool IsNight()
	{
		var currentTime = DateTime.Now;
		return currentTime.Hour is >= 18 or < 6;
	}

	async Task SaveStreamToFileSystem(MemoryStream stream, string fileName)
	{
		if (string.IsNullOrWhiteSpace(RecordingsFolder) || !SaveRecordingToFileStorage)
		{
			return;
		}

		await using var file = new FileStream(Path.Combine(RecordingsFolder, fileName), FileMode.Create, FileAccess.Write);
		await file.WriteAsync(stream.ToArray());
	}

	async partial void OnSaveRecordingToFileStorageChanged(bool value)
	{
		if (value)
		{
			var pickResult = await FolderPicker.PickAsync(CancellationToken.None);
			if (pickResult.IsSuccessful)
			{
				RecordingsFolder = pickResult.Folder.Path;
			}
		}
		else
		{
			RecordingsFolder = null;
		}

		Preferences.Set(nameof(RecordingsFolder), RecordingsFolder);
	}
}