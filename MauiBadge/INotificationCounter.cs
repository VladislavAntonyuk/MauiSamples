namespace MauiBadge;

public interface INotificationCounter
{
	void SetNotificationCount(int count);
}

public static class NotificationCounter
{
	static INotificationCounter? defaultImplementation;

	public static void SetNotificationCount(int count)
	{
		Default.SetNotificationCount(count);
	}

	public static INotificationCounter Default =>
		defaultImplementation ??= new NotificationCounterImplementation();

	internal static void SetDefault(INotificationCounter? implementation) =>
		defaultImplementation = implementation;
}