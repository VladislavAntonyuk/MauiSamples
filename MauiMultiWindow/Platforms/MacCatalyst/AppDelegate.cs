namespace MauiMultiWindow;

using Foundation;
using Microsoft.Maui.LifecycleEvents;
using SpriteKit;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}