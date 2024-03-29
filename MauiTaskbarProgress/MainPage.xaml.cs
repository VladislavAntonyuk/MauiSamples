﻿namespace MauiTaskbarProgress;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
	{
#if MACCATALYST


#elif WINDOWS
		int maxProgressbarValue = 100;
		var taskbarInstance = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
		taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
		taskbarInstance.SetProgressValue((int)e.NewValue, maxProgressbarValue);

		if (e.NewValue >= maxProgressbarValue)
		{
			taskbarInstance.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
		}
#endif
	}
}