namespace MauiMultiWindow;

public static class WindowExtensions
{
	public static Task OpenModalWindow(this Window parentWindow, IView content)
	{
		return Task.CompletedTask;

	}
}