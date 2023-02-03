namespace KanbanBoard;
using Android.App;
using Android.Content.PM;

[Activity(
	Theme = "@style/Maui.SplashTheme",
	MainLauncher = true,
	Label = "KanbanBoard",
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}