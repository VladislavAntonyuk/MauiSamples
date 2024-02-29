namespace Client.UITests;

using System.Diagnostics;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

public sealed class AppiumSetup : IDisposable
{
	private readonly ITestOutputHelper testOutputHelper;
	public const string Platform = "Windows";
	private readonly AppiumServiceHelper appiumService;
	private readonly Process process;

	public AppiumDriver App { get; }

	public AppiumSetup(ITestOutputHelper testOutputHelper)
	{
		this.testOutputHelper = testOutputHelper;
		appiumService = new AppiumServiceHelper();
		appiumService.StartAppiumLocalServer();

		process = Process.Start("explorer.exe", "client:");

		var options = new AppiumOptions
		{
			AutomationName = "windows",
			PlatformName = Platform,
			App = GetApp()
		};

		App = new WindowsDriver(options);
	}

	public void Dispose()
	{
		process.Kill();
		App.Quit();
		appiumService.Dispose();
	}

	private string GetApp()
	{
#if DEBUG
		const string configuration = "Debug";
#else
		const string configuration = "Release";
#endif
		const string testsPath = $@"Client.Windows.UITests\bin\{configuration}\net8.0";
		var solutionPath = Environment.CurrentDirectory.Replace(testsPath, string.Empty);
		var path = $@"{solutionPath}MauiTests\Client\bin\{configuration}\\net8.0-windows10.0.19041.0\\win10-x64\\Client.exe";
		testOutputHelper.WriteLine(path);
		return path;
	}
}