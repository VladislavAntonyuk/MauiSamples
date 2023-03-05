namespace MauiMultiWindow;
using Foundation;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

public static class WindowExtensions
{
	public static void OpenModalWindow(this Window parentWindow, Window modalWindow)
	{

	}
}