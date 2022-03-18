namespace MauiMultiWindow;

public partial class SecondPage : ContentPage
{
	public SecondPage()
	{
		InitializeComponent();
	}

	private void CloseClicked(object sender, EventArgs e)
	{
		Application.Current?.CloseWindow(GetParentWindow());
	}
}