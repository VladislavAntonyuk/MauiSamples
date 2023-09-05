namespace MauiApplicationInsights;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

public class ApplicationInitializer : ITelemetryInitializer
{
	public string SessionId { get; } = Guid.NewGuid().ToString();
	public string? DeviceOperationSystem { get; } = DeviceInfo.Current.Platform.ToString();
	public string DeviceOemName { get; } = DeviceInfo.Current.Manufacturer;
	public string DeviceModel { get; } = DeviceInfo.Current.Model;
	public string ComponentVersion { get; } = AppInfo.Current.VersionString;

	public void Initialize(ITelemetry telemetry)
	{
		telemetry.Context.Session.Id = SessionId;
		telemetry.Context.Device.OperatingSystem = DeviceOperationSystem;
		telemetry.Context.Device.OemName = DeviceOemName;
		telemetry.Context.Device.Model = DeviceModel;
		telemetry.Context.Component.Version = ComponentVersion;
	}
}