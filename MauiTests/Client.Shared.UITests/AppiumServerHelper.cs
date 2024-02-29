namespace Client.UITests;

using OpenQA.Selenium.Appium.Service;

public sealed class AppiumServiceHelper : IDisposable
{
	private const string DefaultHostAddress = "127.0.0.1";
	private const int DefaultHostPort = 4723;

	private readonly AppiumLocalService appiumLocalService;

	public AppiumServiceHelper(string host = DefaultHostAddress, int port = DefaultHostPort)
	{
		var builder = new AppiumServiceBuilder()
		              .WithIPAddress(host)
		              .UsingPort(port);

		appiumLocalService = builder.Build();
	}

	public void StartAppiumLocalServer()
	{
		if (appiumLocalService.IsRunning)
		{
			return;
		}
		
		appiumLocalService.Start();
	}

	public void Dispose()
	{
		appiumLocalService.Dispose();
	}
}