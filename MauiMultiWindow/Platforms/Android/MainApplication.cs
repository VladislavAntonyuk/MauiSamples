namespace MauiMultiWindow;

using Android.App;
using Android.Runtime;

[Application(ResizeableActivity = true)]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}