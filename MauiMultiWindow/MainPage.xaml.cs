namespace MauiMultiWindow;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OpenClicked(object sender, EventArgs e)
	{
		var newWindow = new Window(new SecondPage());
		Application.Current?.OpenWindow(newWindow);
	}

	private void OpenModalClicked(object sender, EventArgs e)
	{
		var newWindow = new Window(new SecondPage() { Title = "I am modal"});
		GetParentWindow().OpenModalWindow(newWindow);
	}

	private void CloseAllClicked(object sender, EventArgs e)
	{
		var windows = Application.Current?.Windows.Skip(1).ToArray();
		foreach (var window in windows ?? Array.Empty<Window>())
		{
			Application.Current?.CloseWindow(window);
		}
	}
}