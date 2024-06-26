namespace MauiNotifications;

using Microsoft.Azure.NotificationHubs;

public partial class DeviceInstallationService
{
	public static string GetDeviceId()
	{
		return string.Empty;
	}

	private static Task<Installation?> GetInstallation()
	{
		return Task.FromResult<Installation?>(null);
	}
}