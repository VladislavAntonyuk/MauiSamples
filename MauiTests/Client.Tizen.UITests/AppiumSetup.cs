namespace Client.UITests;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Tizen;

public sealed class AppiumSetup : IDisposable
{
	private readonly ITestOutputHelper testOutputHelper;
	public const string Platform = "Tizen";
	private readonly AppiumServiceHelper appiumService;

	public AppiumDriver App { get; }

	public AppiumSetup(ITestOutputHelper testOutputHelper)
	{
		this.testOutputHelper = testOutputHelper;
		appiumService = new AppiumServiceHelper();
		appiumService.StartAppiumLocalServer();

		var options = new AppiumOptions
		{
			AutomationName = "Tizen",
			PlatformName = Platform,
			App = GetApp()
		};

		App = new TizenDriver(options);
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
		const string testsPath = $@"Client.Tizen.UITests\bin\{configuration}\net8.0";
		var solutionPath = Environment.CurrentDirectory.Replace(testsPath, string.Empty);
		var path = $@"{solutionPath}Client\bin\{configuration}\\net8.0-tizen\tizen-x86\com.vladislavantonyuk.client-1.0.0.tpk";
		testOutputHelper.WriteLine(path);
		return path;
	}
}