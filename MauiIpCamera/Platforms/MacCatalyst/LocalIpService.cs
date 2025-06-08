namespace MauiIpCamera;

using System.Net;
using System.Net.Sockets;

public class LocalIpService : ILocalIpService
{
	public IPAddress GetLocalIpAddress()
	{
		return Dns.GetHostEntry(Dns.GetHostName())
		          .AddressList
		          .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
		       ?? IPAddress.Loopback;
	}
}