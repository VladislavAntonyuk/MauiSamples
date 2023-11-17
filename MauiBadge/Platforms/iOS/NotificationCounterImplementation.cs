namespace MauiBadge;

using UIKit;
using UserNotifications;

public class NotificationCounterImplementation : INotificationCounter
{
	public void SetNotificationCount(int count)
	{
		UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Badge, (isAuthorized, _) =>
		{
			if (!isAuthorized)
			{
				return;
			}

			if (OperatingSystem.IsIOSVersionAtLeast(16))
			{
				UNUserNotificationCenter.Current.SetBadgeCount(new IntPtr(count), null);
			}
			else
			{
				UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
			}
		});
	}
}