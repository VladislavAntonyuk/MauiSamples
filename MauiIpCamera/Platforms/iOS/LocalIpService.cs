namespace MauiIpCamera;

using System.Net;
using System.Net.Sockets;

public class LocalIpService : ILocalIpService
{
	public IPAddress GetLocalIpAddress()
	{
		// First try to get the WiFi IP address which is more likely to be accessible
		var addresses = Dns.GetHostEntry(Dns.GetHostName())
		                  .AddressList
		                  .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
		                  .Select(ip => ip.ToString())
		                  .ToList();

		// Filter out loopback addresses
		var nonLoopbackAddresses = addresses.Where(a => !a.StartsWith("127.")).ToList();

		// Prefer addresses that start with 192.168 or 10. (common local networks)
		var preferredAddress = nonLoopbackAddresses.FirstOrDefault(a => a.StartsWith("192.168.") || a.StartsWith("10."));

		// Fall back to any non-loopback address if no preferred format was found
		if (string.IsNullOrEmpty(preferredAddress) && nonLoopbackAddresses.Any())
		{
			preferredAddress = nonLoopbackAddresses.First();
		}

		// Last resort - use loopback address if nothing else is available
		return  preferredAddress is null ? IPAddress.Loopback :  IPAddress.Parse(preferredAddress);
	}
}