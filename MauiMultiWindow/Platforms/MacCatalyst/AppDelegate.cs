namespace MauiMultiWindow;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using SpriteKit;
using UIKit;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

public static class WindowExtensions
{
	public static void OpenModalWindow(this Window parentWindow, Window modalWindow)
	{
		var parentUIWindow = parentWindow.Handler.PlatformView as UIWindow ?? throw new Exception();
		var modalUIWindow = modalWindow.ToPlatform(parentWindow.Handler.MauiContext!) as UIWindow ?? throw new Exception();
		//parentUIWindow.RootViewController?.PresentViewController(modalUIWindow.RootViewController ?? throw new Exception(), true, null) ;
		var modalViewController = modalUIWindow.RootViewController ?? throw new Exception();
#pragma warning disable CA1422 // Validate platform compatibility
		//parentUIWindow.RootViewController?.PresentModalViewController(modalViewController, true);
#pragma warning restore CA1422 // Validate platform compatibility

		//	UIApplication.SharedApplication.RequestSceneSessionActivation(parentUIWindow.WindowScene!.Session, modalUIWindow.UserActivity, null, null);

		var window = CreatePlatformWindow(Application.Current!, modalUIWindow.WindowScene, null);
		var scenes = UIApplication.SharedApplication.ConnectedScenes.ToArray().Select(x=>x.Delegate).ToList();
		//.SetWindow(window!);
		UIApplication.SharedApplication.Delegate.GetWindow()?.MakeKeyAndVisible();
	}

	static UIWindow? CreatePlatformWindow(IApplication application, UIWindowScene? windowScene, NSDictionary[]? states)
	{
		if (application.Handler?.MauiContext is not IMauiContext applicationContext)
			return null;

		var uiWindow = windowScene is not null
#pragma warning disable CA1416 // UIWindow(windowScene) is only supported on: ios 13.0 and later
			? new UIWindow(windowScene)
#pragma warning restore CA1416
			: new UIWindow();

		var activationState = new ActivationState(applicationContext, states);

		var mauiWindow = application.CreateWindow(activationState);

		uiWindow.SetWindowHandler(mauiWindow, applicationContext);

		return uiWindow;
	}
}