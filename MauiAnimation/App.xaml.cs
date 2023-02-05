namespace MauiAnimation;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		if (DeviceInfo.Idiom == DeviceIdiom.Phone)
		{
			MainPage = new ContentPage()
			{
				Content = new Label()
				{
					Text = "This sample is not designed to be used on phones.",
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center
				}
			};
		}
		else
		{
			MainPage = new AppShell();
		}
	}
}