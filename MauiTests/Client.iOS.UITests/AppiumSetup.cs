namespace Client.UITests;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

public sealed class AppiumSetup : IDisposable
{
	private readonly ITestOutputHelper testOutputHelper;
	public const string Platform = "iOS";
	private readonly AppiumServiceHelper appiumService;

	public AppiumDriver App { get; }

	public AppiumSetup(ITestOutputHelper testOutputHelper)
	{
		this.testOutputHelper = testOutputHelper;
		appiumService = new AppiumServiceHelper();
		appiumService.StartAppiumLocalServer();

		var options = new AppiumOptions
		{
			AutomationName = "XCUITest",
			PlatformName = Platform,
			PlatformVersion = "17.0",
			DeviceName = "iPhone 15 Pro",
			App = GetApp()
		};

		App = new IOSDriver(options);
	}

	public void Dispose()
	{
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
		const string testsPath = $@"Client.iOS.UITests\bin\{configuration}\net8.0";
		var solutionPath = Environment.CurrentDirectory.Replace(testsPath, string.Empty);
		var path = $@"{solutionPath}Client\bin\{configuration}\net8.0-ios\iossimulator-x64\Client.app";
		testOutputHelper.WriteLine(path);
		return path;
	}
}