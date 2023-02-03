namespace MauiBadge;

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

public class NotificationCounterImplementation : INotificationCounter
{
	public void SetNotificationCount(int count)
	{
		var badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
		if (count <= 0)
		{
			badgeUpdater.Clear();
		}
		else
		{
			var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);

			var badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
			badgeElement?.SetAttribute("value", count.ToString());

			var badge = new BadgeNotification(badgeXml);
			badgeUpdater.Update(badge);
		}
	}
}