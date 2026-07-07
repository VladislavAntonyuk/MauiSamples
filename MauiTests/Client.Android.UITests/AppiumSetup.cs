namespace Client.UITests;

using AndroidSdk;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using PercyIO.Appium;

public sealed class AppiumSetup : IDisposable
{
	private readonly ITestOutputHelper testOutputHelper;
	public const string Platform = "Android";
	private const string AvdName = "CI_Emulator";
	private const string PackageName = "com.vladislavantonyuk.client";

	private readonly AppiumServiceHelper appiumService;
	private readonly Emulator.AndroidEmulatorProcess emulatorProcess;

	public AppiumDriver App { get; }

	public AppPercy Percy { get; }

	public AppiumSetup(ITestOutputHelper testOutputHelper)
	{
		this.testOutputHelper = testOutputHelper;
		var sdk = InstallSoftware();
		emulatorProcess = sdk.Emulator.Start(AvdName, new Emulator.EmulatorStartOptions { NoSnapshot = true });
		emulatorProcess.WaitForBootComplete();

		appiumService = new AppiumServiceHelper();
		appiumService.StartAppiumLocalServer();

		var options = new AppiumOptions
		{
			AutomationName = "UIAutomator2",
			PlatformName = Platform,
			PlatformVersion = "15",
			App = GetApp()
		};
		Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
		browserstackOptions.Add("userName", "");
		browserstackOptions.Add("accessKey", "");

		//percyOptions
		Dictionary<string, string> percyOtions = new Dictionary<string, string>();
		percyOtions.Add("ignoreErrors", "true");
		percyOtions.Add("enabled", "true");
		// Adding capabilities
		options.AddAdditionalAppiumOption("bstack:options", browserstackOptions);
		App = new AndroidDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub"), options);
		Percy = new AppPercy(App);
	}

	public void Dispose()
	{
		App.Quit();
		emulatorProcess.Shutdown();
		appiumService.Dispose();
	}

	private static AndroidSdkManager InstallSoftware()
	{
		const string avdSdkId = "system-images;android-35;google_apis_playstore;x86_64";

		var sdkPackages = new[]
		{
			"platforms;android-35"
		};

		var sdk = new AndroidSdkManager();
		sdk.Acquire();
		sdk.SdkManager.Install(sdkPackages);
		sdk.SdkManager.Install(avdSdkId);
		if (sdk.AvdManager.ListAvds().All(x => x.Name != AvdName))
		{
			sdk.AvdManager.Create(AvdName, avdSdkId, "pixel", force: true);
		}

		return sdk;
	}

	private string GetApp()
	{
#if DEBUG
		const string configuration = "Debug";
#else
		const string configuration = "Release";
#endif
		const string testsPath = $@"Client.Android.UITests\bin\{configuration}\net10.0";
		var solutionPath = Environment.CurrentDirectory.Replace(testsPath, string.Empty);
		var path = $@"{solutionPath}Client\bin\{configuration}\net10.0-android\{PackageName}-Signed.apk";
		testOutputHelper.WriteLine(path);
		return path;
	}
}