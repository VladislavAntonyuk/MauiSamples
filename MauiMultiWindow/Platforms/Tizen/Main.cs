namespace MauiMultiWindow;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}

//public static class WindowExtensions
//{
//	public static Task OpenModalWindow(this Window parentWindow, Window modalWindow)
//	{
//		return Task.CompletedTask;
//	}
//}