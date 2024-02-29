namespace Client.UITests;

public class LoginPageTests(ITestOutputHelper testOutputHelper) : BaseTest(testOutputHelper)
{
	[AllowOnPlatformFact(
		RuntimePlatform.WindowsAndroid,
		RuntimePlatform.OsxAndroid,
		RuntimePlatform.OsxIOs,
		RuntimePlatform.OsxMacCatalyst,
		RuntimePlatform.WindowsWindows,
		RuntimePlatform.OsxTizen
	)]
	public async Task AppLaunches()
	{
		await Task.Delay(2000);
		VerifyScreenshot($"{nameof(AppLaunches)}");
	}

	[AllowOnPlatformFact(
		RuntimePlatform.WindowsAndroid,
		RuntimePlatform.OsxAndroid,
		RuntimePlatform.OsxIOs,
		RuntimePlatform.OsxMacCatalyst,
		RuntimePlatform.WindowsWindows,
		RuntimePlatform.OsxTizen
	)]
	public async Task LoginBtnTest()
	{
		// Arrange
		var element = FindUiElement("CounterBtn");

		// Act & Assert
		element.Click();
		await Task.Delay(500);
		Assert.Equal("Clicked 1 time", element.Text);

		element.Click();
		await Task.Delay(500);
		Assert.Equal("Clicked 2 times", element.Text);
	}
}