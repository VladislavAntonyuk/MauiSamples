namespace MauiBadge;

using UIKit;
using UserNotifications;

public class NotificationCounterImplementation : INotificationCounter
{
	public void SetNotificationCount(int count)
	{
		UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Badge, (r, e) =>
		{
		});
		UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
	}
}