namespace MauiNotifications;

using System.Diagnostics;
using Windows.Networking.PushNotifications;
using Windows.Security.ExchangeActiveSyncProvisioning;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Azure.NotificationHubs;

public partial class DeviceInstallationService
{
	public static string GetDeviceId()
	{
		return new EasClientDeviceInformation().Id.ToString();
	}

	private static async Task<Installation?> GetInstallation()
	{
		var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
		channel.PushNotificationReceived += Channel_PushNotificationReceived;
		return new Installation
		{
			InstallationId = GetDeviceId(),
			Platform = NotificationPlatform.Wns,
			PushChannel = channel.Uri
		};
	}

	private static async void Channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
	{
		await Toast.Make(args.RawNotification.Content).Show();
	}
}