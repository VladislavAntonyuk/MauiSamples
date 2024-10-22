namespace Client.UITests;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Mac;

public sealed class AppiumSetup : IDisposable
{
	private readonly ITestOutputHelper testOutputHelper;
	public const string Platform = "Mac";
	private readonly AppiumServiceHelper appiumService;

	public AppiumDriver App { get; }

	public AppiumSetup(ITestOutputHelper testOutputHelper)
	{
		this.testOutputHelper = testOutputHelper;
		appiumService = new AppiumServiceHelper();
		appiumService.StartAppiumLocalServer();

		var options = new AppiumOptions
		{
			AutomationName = "mac2",
			PlatformName = Platform,
			App = GetApp()
		};

		options.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, "com.vladislavantonyuk.client");
		App = new MacDriver(options);
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
		const string testsPath = $@"Client.MacCatalyst.UITests\bin\{configuration}\net9.0";
		var solutionPath = Environment.CurrentDirectory.Replace(testsPath, string.Empty);
		var path = $@"{solutionPath}Client\bin\{configuration}\net9.0-maccatalyst\maccatalyst-x64\Client.app";
		testOutputHelper.WriteLine(path);
		return path;
	}
}