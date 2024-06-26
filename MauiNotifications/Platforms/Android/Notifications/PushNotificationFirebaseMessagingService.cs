namespace MauiNotifications;

using Android;
using Android.App;
using Android.Content.PM;
using AndroidX.Core.Content;
using Firebase.Messaging;

[Service(Exported = false)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class PushNotificationFirebaseMessagingService : FirebaseMessagingService
{
	int messageId;
	public override void OnMessageReceived(RemoteMessage message)
	{
		base.OnMessageReceived(message);
		MainThread.InvokeOnMainThreadAsync(() =>
		{
			if (OperatingSystem.IsAndroidVersionAtLeast(33)
				&& ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != Permission.Granted)
			{
				return;
			}

			if (!OperatingSystem.IsAndroidVersionAtLeast(26))
			{
				return;
			}

			var pushNotification = message.GetNotification();

			var manager = (NotificationManager?)Application.Context.GetSystemService(NotificationService);
			var channel = new NotificationChannel(pushNotification.ChannelId ?? "MauiNotifications", "MauiNotifications", NotificationImportance.Max);
			manager?.CreateNotificationChannel(channel);

			var notification = new Notification.Builder(Application.Context, channel.Id)
										.SetContentTitle(pushNotification.Title)
										.SetContentText(pushNotification.Body)
										.SetSmallIcon(Android.Resource.Mipmap.SymDefAppIcon)
										.Build();

			manager?.Notify(messageId++, notification);
		});
	}
}