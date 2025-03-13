namespace MauiMultiWindow;

public static partial class WindowExtensions
{
	public static bool IsActive(this Window window)
	{
		return window is WindowEx { IsActive: true };
	}

	public static Window GetActiveWindow(this IApplication application)
	{
		return application.Windows.OfType<WindowEx>().First(x => x.IsActive);
	}
}