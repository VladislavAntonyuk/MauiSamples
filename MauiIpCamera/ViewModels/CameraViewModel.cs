namespace MauiIpCamera.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;

public partial class CameraViewModel : ObservableObject
{
	const string Mjpeg = "MJPEG";
	const string Video = "VIDEO";
	private readonly IAutoStartService autoStartService;
	private readonly IPreferences preferences;
	private const int Port = 5555;
	private readonly string ipAddress;
	private bool isFirstStart = true;

	private readonly LocalHttpServer server;

	public CameraViewModel(ILocalIpService localIpService, IAutoStartService autoStartService, IPreferences preferences)
	{
		this.autoStartService = autoStartService;
		this.preferences = preferences;
		var localIp = localIpService.GetLocalIpAddress();
		server = new LocalHttpServer(localIp, Port, Frequency);
		IpAddressText = ipAddress = $"{localIp}:{Port}";

		AvailableResolutions = [];
		AvailableModes = [Video, Mjpeg];
		SelectedMode = preferences.Get(nameof(SelectedMode), AvailableModes.First());
		MaxConnectionsCount = preferences.Get(nameof(MaxConnectionsCount), 10);
		VideoDuration = preferences.Get(nameof(VideoDuration), 10);
		Frequency = preferences.Get(nameof(Frequency), 10);
		RecordingsFolder = preferences.Get<string?>(nameof(RecordingsFolder), null);
		if (!string.IsNullOrWhiteSpace(RecordingsFolder))
		{
			SaveRecordingToFileStorage = true;
		}
		else
		{
			isFirstStart = false;
		}

		IsAutoStartEnabled = preferences.Get(nameof(IsAutoStartEnabled), autoStartService.IsAutoStartEnabledAsync().GetAwaiter().GetResult());
	}

	[ObservableProperty]
	public partial int MaxConnectionsCount { get; set; }

	[ObservableProperty]
	public partial int VideoDuration { get; set; }

	[ObservableProperty]
	public partial int MaxFiles { get; set; }

	[ObservableProperty]
	public partial int Frequency { get; set; }

	[ObservableProperty]
	public partial bool IsAutoStartEnabled { get; set; }

	[ObservableProperty]
	public partial bool SaveRecordingToFileStorage { get; set; }

	[ObservableProperty]
	public partial string? RecordingsFolder { get; set; }

	[ObservableProperty]
	public partial bool IsPowerSavingModeEnabled { get; set; }

	[ObservableProperty]
	public partial string IpAddressText { get; set; }

	public ObservableCollection<Size> AvailableResolutions { get; }

	public ObservableCollection<string> AvailableModes { get; }

	[ObservableProperty]
	public partial string? SelectedMode { get; set; }

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
	void OpenSettings()
	{
		var popup = new SettingsPage(this);
		Shell.Current.ShowPopup(popup, new PopupOptions() { CanBeDismissedByTappingOutsideOfPopup = true });
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
		_ = server.StartAsync(MaxConnectionsCount, cancellationToken);
		switch (SelectedMode)
		{
			case Mjpeg:
				IpAddressText = $"http://{ipAddress}/mjpeg";
				await CaptureMjpegAsync(cameraView, cancellationToken);
				break;
			case Video:
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
			await Task.Delay(Frequency, CancellationToken.None);
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
			await Task.Delay(TimeSpan.FromSeconds(VideoDuration), CancellationToken.None);
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
		if (string.IsNullOrWhiteSpace(RecordingsFolder) || !SaveRecordingToFileStorage)
		{
			return;
		}


		await using var recording = new FileStream(Path.Combine(RecordingsFolder, fileName), FileMode.Create, FileAccess.Write);
		await recording.WriteAsync(stream.ToArray());

		var files = Directory.GetFiles(RecordingsFolder, "*.mp4");
		if (files.Length >= MaxFiles)
		{
			var filesToDelete = files
				.OrderByDescending(f => new FileInfo(f).CreationTime)
				.Skip(MaxFiles - 1)
				.ToList();
			foreach (var file in filesToDelete)
			{
				File.Delete(file);
			}
		}
	}

	async partial void OnIsAutoStartEnabledChanged(bool value)
	{
		if (value)
		{
			await autoStartService.EnableAutoStartAsync();
		}
		else
		{
			await autoStartService.DisableAutoStartAsync();
		}

		preferences.Set(nameof(IsAutoStartEnabled), await autoStartService.IsAutoStartEnabledAsync());
	}

	partial void OnSelectedModeChanged(string? value)
	{
		preferences.Set(nameof(SelectedMode), value);
	}

	partial void OnMaxConnectionsCountChanged(int value)
	{
		preferences.Set(nameof(MaxConnectionsCount), value);
	}

	partial void OnVideoDurationChanged(int value)
	{
		preferences.Set(nameof(VideoDuration), value);
	}

	partial void OnFrequencyChanged(int value)
	{
		preferences.Set(nameof(Frequency), value);
	}

	async partial void OnSaveRecordingToFileStorageChanged(bool value)
	{
		if (isFirstStart)
		{
			isFirstStart = false;
			return;
		}

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

		preferences.Set(nameof(RecordingsFolder), RecordingsFolder);
	}
}