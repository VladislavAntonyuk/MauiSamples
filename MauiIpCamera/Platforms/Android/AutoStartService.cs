namespace MauiIpCamera;

using Android.App;
using Android.Content;
using Android.Widget;
using CommunityToolkit.Maui.Alerts;
using Toast = Android.Widget.Toast;

public class AutoStartService : IAutoStartService
{
	public const string PreferencesKey = "IsAutoStartEnabled";
	public Task<bool> IsAutoStartEnabledAsync()
	{
		var result = Preferences.Get(PreferencesKey, false);
		return Task.FromResult(result);
	}

	public Task<bool> EnableAutoStartAsync()
	{
		Preferences.Set(PreferencesKey, true);
		return Task.FromResult(true);
	}

	public Task<bool> DisableAutoStartAsync()
	{
		Preferences.Set(PreferencesKey, false);
		return Task.FromResult(true);
	}
}