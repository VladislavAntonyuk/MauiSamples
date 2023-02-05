namespace MauiBadge;

using Android.App;
using AndroidX.Core.App;

public class NotificationCounterImplementation : INotificationCounter
{
	public void SetNotificationCount(int count)
	{
		ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(Application.Context, count);
		NotificationCompat.Builder builder = new(Application.Context, $"{Application.Context.PackageName}.channel");
		builder.SetNumber(count);
		builder.SetContentTitle(" ");
		builder.SetContentText("");
		builder.SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon);
		var notification = builder.Build();
		var notificationManager = NotificationManager.FromContext(Application.Context);
		CreateNotificationChannel();
		notificationManager?.Notify((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), notification);
	}

	private static void CreateNotificationChannel()
	{
		if (OperatingSystem.IsAndroidVersionAtLeast(26))
		{
			using var channel = new NotificationChannel($"{Application.Context.PackageName}.channel", "Notification channel", NotificationImportance.Default)
			{
				Description = "Default notification channel"
			};
			var notificationManager = NotificationManager.FromContext(Application.Context);
			notificationManager?.CreateNotificationChannel(channel);
		}
	}
}