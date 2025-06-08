namespace MauiIpCamera;

using System.Net;

public interface ILocalIpService
{
	IPAddress GetLocalIpAddress();
}