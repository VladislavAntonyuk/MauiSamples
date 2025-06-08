namespace MauiIpCamera;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class LocalIpService : ILocalIpService
{
	public IPAddress GetLocalIpAddress()
	{
		return NetworkInterface.GetAllNetworkInterfaces()
		                        .SelectMany(x => x.GetIPProperties().UnicastAddresses)
		                        .Where(x => x.Address.AddressFamily == AddressFamily.InterNetwork && x.IsDnsEligible)
		                        .Select(x => x.Address)
		                        .FirstOrDefault(IPAddress.Loopback);
	}
}