namespace MauiCursor;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		Hover.IsVisible = true;
		Hover.SetCustomCursor(CursorIcon.Wait);
	}
}