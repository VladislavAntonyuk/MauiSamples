namespace MauiNotifications;

using Microsoft.Azure.NotificationHubs;

public partial class DeviceInstallationService(IConnectivity connectivity, INotificationHubClient notificationHubClient)
{
	public async Task RegisterDevice()
	{
		if (connectivity.NetworkAccess != NetworkAccess.Internet)
		{
			return;
		}

		var installation = await GetInstallation();
		if (installation == null)
		{
			return;
		}

		await notificationHubClient.CreateOrUpdateInstallationAsync(installation);
	}
}