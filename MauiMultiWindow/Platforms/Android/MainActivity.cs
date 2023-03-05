namespace MauiMultiWindow;
using Android.App;
using Android.Content.PM;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}

public static class WindowExtensions
{
	public static void OpenModalWindow(this Window parentWindow, Window modalWindow)
	{

	}
}