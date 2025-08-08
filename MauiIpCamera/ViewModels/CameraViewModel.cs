namespace MauiIpCamera.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;

public partial class CameraViewModel : ObservableObject
{
	private readonly IPreferences preferences;
	private readonly SettingsViewModel settingsViewModel;
	private const int Port = 5555;
	private readonly string ipAddress;
	private IReadOnlyList<CameraInfo> availableCameras = [];

	private readonly LocalHttpServer server;
	private readonly int frequency;

	public CameraViewModel(ILocalIpService localIpService, IPreferences preferences, SettingsViewModel settingsViewModel)
	{
		this.preferences = preferences;
		this.settingsViewModel = settingsViewModel;
		var localIp = localIpService.GetLocalIpAddress();
		frequency = preferences.Get(nameof(SettingsViewModel.Frequency), 100);
		server = new LocalHttpServer(localIp, Port, frequency);
		IpAddressText = ipAddress = $"{localIp}:{Port}";

		AvailableResolutions = [];
	}

	public bool IsPowerSavingModeEnabled
	{
		get => settingsViewModel.IsPowerSavingModeEnabled;
		set
		{
			settingsViewModel.IsPowerSavingModeEnabled = value;
			OnPropertyChanged();
		}
	}

	[ObservableProperty]
	public partial string IpAddressText { get; set; }

	public ObservableCollection<Size> AvailableResolutions { get; }

	[ObservableProperty]
	public partial Size SelectedResolution { get; set; } = new(800, 600);

	public async Task InitializeCameraAsync(CameraView cameraView)
	{
		var cameraRequest = await Permissions.RequestAsync<Permissions.Camera>();
		var microphoneRequest = await Permissions.RequestAsync<Permissions.Microphone>();
		if (cameraRequest != PermissionStatus.Granted || microphoneRequest != PermissionStatus.Granted)
		{
			await Shell.Current.CurrentPage.DisplayAlert("Permission Denied", "Camera and Microphone permissions are required to use this feature.", "OK");
			return;
		}

		availableCameras = await cameraView.GetAvailableCameras(CancellationToken.None);

		cameraView.SelectedCamera = availableCameras.FirstOrDefault();

		AvailableResolutions.Clear();

		if (cameraView.SelectedCamera?.SupportedResolutions != null)
		{
			foreach (var resolution in cameraView.SelectedCamera.SupportedResolutions)
			{
				AvailableResolutions.Add(resolution);
			}
		}

		await cameraView.StartCameraPreview(CancellationToken.None);
	}

	[RelayCommand(AllowConcurrentExecutions = false)]
	async Task OpenSettings()
	{
		var popup = new SettingsPage(settingsViewModel);
		await Shell.Current.ShowPopupAsync(popup, new PopupOptions() { CanBeDismissedByTappingOutsideOfPopup = true });
	}

	[RelayCommand]
	void DisablePowerSavingMode()
	{
		IsPowerSavingModeEnabled = false;
	}

	[RelayCommand(AllowConcurrentExecutions = false, IncludeCancelCommand = true)]
	private async Task StartStream(CameraView cameraView, CancellationToken cancellationToken)
	{
		DeviceDisplay.KeepScreenOn = true;
		var selectedMode = preferences.Get(nameof(SettingsViewModel.SelectedMode), SettingsViewModel.Mjpeg);
		var maxConnectionsCount = preferences.Get(nameof(SettingsViewModel.MaxConnectionsCount), 1);
		switch (selectedMode)
		{
			case SettingsViewModel.Mjpeg:
				_ = server.StartAsync(maxConnectionsCount, cancellationToken);
				IpAddressText = $"http://{ipAddress}/mjpeg";
				await CaptureMjpegAsync(cameraView, cancellationToken);
				break;
			case SettingsViewModel.Video:
				_ = server.StartAsync(maxConnectionsCount, cancellationToken);
				IpAddressText = $"http://{ipAddress}/player";
				await CaptureVideoAsync(cameraView, cancellationToken);
				break;
		}

		DeviceDisplay.KeepScreenOn = false;
	}

	private async Task CaptureMjpegAsync(CameraView cameraView, CancellationToken cancellationToken)
	{
#if ANDROID
		var mode = IsNight() ? AndroidX.Camera.Extensions.ExtensionMode.Night : AndroidX.Camera.Extensions.ExtensionMode.Auto;
		await cameraView.SetExtensionMode(mode);
#endif
		cameraView.ImageCaptureResolution = SelectedResolution;
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
			await Task.Delay(frequency, CancellationToken.None);
		}

		server.Stop();
	}

	private async Task CaptureVideoAsync(CameraView cameraView, CancellationToken cancellationToken)
	{
#if ANDROID
		var mode = IsNight() ? AndroidX.Camera.Extensions.ExtensionMode.Night : AndroidX.Camera.Extensions.ExtensionMode.Auto;
		await cameraView.SetExtensionMode(mode);
#endif
		cameraView.ImageCaptureResolution = SelectedResolution;
		var videoDuration = preferences.Get(nameof(SettingsViewModel.VideoDuration), 10);
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
			await Task.Delay(TimeSpan.FromSeconds(videoDuration), CancellationToken.None);
			await cameraView.StopVideoRecording(CancellationToken.None);
			server.SetVideoStream(stream);
			await SaveStreamToFileSystem(stream, $"recording-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}.mp4");
		}

		server.Stop();
	}

	private static bool IsNight()
	{
		var currentTime = DateTime.Now;
		return currentTime.Hour is >= 18 or < 6;
	}

	async Task SaveStreamToFileSystem(MemoryStream stream, string fileName)
	{
		var recordingsFolder = preferences.Get<string?>(nameof(SettingsViewModel.RecordingsFolder), null);
		var saveRecordingToFileStorage = preferences.Get(nameof(SettingsViewModel.SaveRecordingToFileStorage), false);
		if (string.IsNullOrWhiteSpace(recordingsFolder) || !saveRecordingToFileStorage)
		{
			return;
		}


		await using var recording = new FileStream(Path.Combine(recordingsFolder, fileName), FileMode.Create, FileAccess.Write);
		await recording.WriteAsync(stream.ToArray());

		var files = Directory.GetFiles(recordingsFolder, "*.mp4");
		var maxFiles = preferences.Get(nameof(SettingsViewModel.MaxFiles), 10);
		if (files.Length >= maxFiles)
		{
			var filesToDelete = files
				.OrderByDescending(f => new FileInfo(f).CreationTime)
				.Skip(maxFiles - 1)
				.ToList();
			foreach (var file in filesToDelete)
			{
				File.Delete(file);
			}
		}
	}
}