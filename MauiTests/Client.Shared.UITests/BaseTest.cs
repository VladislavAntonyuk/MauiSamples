namespace Client.UITests;

using System.Globalization;
using System.Runtime.InteropServices;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using VisualTestUtils;
using VisualTestUtils.MagickNet;

public abstract class BaseTest(ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
	[DllImport("user32.dll", SetLastError = true)]
	static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

	private const double DifferenceThreshold = 1 / 100d; // 1% difference
	private readonly VisualRegressionTester visualRegressionTester = new(testRootDirectory: Environment.CurrentDirectory,
																		  visualComparer: new MagickNetVisualComparer(differenceThreshold: DifferenceThreshold),
																		  visualDiffGenerator: new MagickNetVisualDiffGenerator(),
																		  ciArtifactsDirectory: Environment.GetEnvironmentVariable("Build.ArtifactStagingDirectory"));
	private readonly MagickNetImageEditorFactory imageEditorFactory = new();

	protected AppiumDriver App { get; } = new AppiumSetup(testOutputHelper).App;

	protected AppiumElement FindUiElement(string id)
	{
		return App.FindElement(App is WindowsDriver ? MobileBy.AccessibilityId(id) : MobileBy.Id(id));
	}

	public ValueTask InitializeAsync()
	{
		testOutputHelper.WriteLine($"CIArtifactsDirectory: {Environment.GetEnvironmentVariable("Build.ArtifactStagingDirectory")}");
		return ValueTask.CompletedTask;
	}

	public ValueTask DisposeAsync()
	{
		App.Dispose();
		return ValueTask.CompletedTask;
	}

	protected async Task VerifyScreenshot(string name)
	{
		if (App.PlatformName == "Windows")
		{
			var handle = App.CurrentWindowHandle;
			MoveWindow(IntPtr.Parse(handle[2..], NumberStyles.HexNumber), 0, 0, 800, 600, true);
			await Task.Delay(1000);
		}

		var screenshotPngBytes = App.GetScreenshot().AsByteArray;

		var actualImage = new ImageSnapshot(screenshotPngBytes, ImageSnapshotFormat.PNG);

		// For Android and iOS, crop off the OS status bar at the top since it's not part of the
		// app itself and contains the time, which always changes. For WinUI, crop off the title
		// bar at the top as it varies slightly based on OS theme and is also not part of the app.
		int cropFromTop = App.PlatformName switch
		{
			"Android" => 60,
			"iOS" => 90,
			"Windows" => 32,
			_ => 0,
		};

		// For Android also crop the 3 button nav from the bottom, since it's not part of the
		// app itself and the button color can vary (the buttons change clear briefly when tapped)
		int cropFromBottom = App.PlatformName switch
		{
			"Android" => 125,
			_ => 0,
		};

		if (cropFromTop > 0 || cropFromBottom > 0)
		{
			IImageEditor imageEditor = imageEditorFactory.CreateImageEditor(actualImage);
			(int width, int height) = imageEditor.GetSize();

			imageEditor.Crop(0, cropFromTop, width, height - cropFromTop - cropFromBottom);

			actualImage = imageEditor.GetUpdatedImage();
		}

		visualRegressionTester.VerifyMatchesSnapshot(name, actualImage, environmentName: App.PlatformName);
	}
}