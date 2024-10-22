namespace MauiAnimation;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		if (DeviceInfo.Idiom == DeviceIdiom.Phone)
		{
			return new Window(new ContentPage
			{
				Content = new Label
				{
					Text = "This sample is not designed to be used on phones.",
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center
				}
			});
		}

		return new Window(new AppShell());
	}
}