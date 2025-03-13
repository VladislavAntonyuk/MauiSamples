namespace MauiMultiWindow;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new WindowEx(new MainPage())
		{
			Title = "Maui Multi Window"
		};
	}
}