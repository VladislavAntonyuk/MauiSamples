namespace MauiBank;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Plugin.Fingerprint;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
#pragma warning disable CA1416 // Validate platform compatibility
		CrossFingerprint.SetCurrentActivityResolver(() => Platform.CurrentActivity);
#pragma warning restore CA1416 // Validate platform compatibility
		base.OnCreate(savedInstanceState);
	}

	protected override void OnResume()
	{
		Window?.ClearFlags(WindowManagerFlags.Secure);
		base.OnResume();
	}

	protected override void OnPause()
	{
		Window?.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);
		base.OnPause();
	}

}