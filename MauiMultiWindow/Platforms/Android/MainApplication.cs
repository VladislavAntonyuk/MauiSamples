namespace MauiMultiWindow;

using Android.App;
using Android.Runtime;

[Application(ResizeableActivity = true)]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}