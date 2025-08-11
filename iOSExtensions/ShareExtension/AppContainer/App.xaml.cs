namespace AppContainer;

public partial class App : Application
{
	private readonly MainPage _mainPage;

	public App()
	{
		InitializeComponent();
		_mainPage = new MainPage();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(_mainPage);
	}

	public void LoadImage(byte[] imageData)
	{
		_mainPage.LoadImage(imageData);
	}
}