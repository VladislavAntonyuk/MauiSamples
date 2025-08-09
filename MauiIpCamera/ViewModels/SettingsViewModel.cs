namespace MauiIpCamera.ViewModels;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class SettingsViewModel : ObservableObject
{
	public const string Mjpeg = "MJPEG";
	public const string Video = "VIDEO";
	private readonly IAutoStartService autoStartService;
	private readonly IPreferences preferences;
	private bool isFirstStart = true;

	public SettingsViewModel(IAutoStartService autoStartService, IPreferences preferences)
	{
		this.autoStartService = autoStartService;
		this.preferences = preferences;

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

	public ObservableCollection<string> AvailableModes { get; }

	[ObservableProperty]
	public partial string? SelectedMode { get; set; }

	[RelayCommand]
	void DisablePowerSavingMode()
	{
		IsPowerSavingModeEnabled = false;
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