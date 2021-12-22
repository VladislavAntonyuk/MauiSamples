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
}