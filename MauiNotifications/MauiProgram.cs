namespace MauiNotifications;

using CommunityToolkit.Maui;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
		       .UseMauiCommunityToolkit();

		builder.Services.AddSingleton(Connectivity.Current);

		builder.Services.AddSingleton<DeviceInstallationService>();

		builder.Services.AddSingleton<INotificationHubClient, NotificationHubClient>(
			_ => NotificationHubClient.CreateClientFromConnectionString("YOUR CONNECTION STRING", "MauiNotifications", true));

		return builder.Build();
	}
}