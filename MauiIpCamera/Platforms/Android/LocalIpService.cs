namespace MauiIpCamera;

using System.Net;
using Android.Content;
using Android.Net.Wifi;

public class LocalIpService : ILocalIpService
{
	public IPAddress GetLocalIpAddress()
	{
		var context = Android.App.Application.Context;
		var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
		var ip = wifiManager?.ConnectionInfo?.IpAddress ?? 0;

		var ipAddress = string.Format(
			"{0}.{1}.{2}.{3}",
			(ip & 0xFF),
			(ip >> 8) & 0xFF,
			(ip >> 16) & 0xFF,
			(ip >> 24) & 0xFF
		);

		return IPAddress.Parse(ipAddress);
	}
}