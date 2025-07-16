namespace MauiIpCamera;

using System.Net;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Java.Net;

public class LocalIpService : ILocalIpService
{
	public IPAddress GetLocalIpAddress()
	{
		if (OperatingSystem.IsAndroidVersionAtLeast(23))
		{
			var connectivityManager = (ConnectivityManager?)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);

			var activeNetwork = connectivityManager?.ActiveNetwork;
			if (activeNetwork == null)
			{
				return IPAddress.Loopback;
			}

			var linkProperties = connectivityManager?.GetLinkProperties(activeNetwork);
			if (linkProperties?.LinkAddresses == null)
			{
				return IPAddress.Loopback;
			}

			foreach (var linkAddress in linkProperties.LinkAddresses)
			{
				var address = linkAddress.Address;
				if (address is not Inet4Address || address.IsLoopbackAddress)
				{
					continue;
				}

				var hostAddress = address.HostAddress;
				if (!string.IsNullOrEmpty(hostAddress) && IPAddress.TryParse(hostAddress, out var ipAddress))
				{
					return ipAddress;
				}
			}
		}
		else
		{
			var context = Android.App.Application.Context;
			var wifiManager = (WifiManager?)context.GetSystemService(Context.WifiService);
			var ip = wifiManager?.ConnectionInfo?.IpAddress ?? 0;

			var ipAddress = $"{(ip & 0xFF)}.{(ip >> 8) & 0xFF}.{(ip >> 16) & 0xFF}.{(ip >> 24) & 0xFF}";

			return IPAddress.Parse(ipAddress);
		}

		return IPAddress.Loopback;
	}
}