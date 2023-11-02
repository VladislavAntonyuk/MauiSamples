namespace MauiTaskListApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

	private void OnSearchClicked(object sender, EventArgs e)
	{
		DisplayAlert(Title, "Search Tasks", "OK");
	}

	private void OnAboutClicked(object sender, EventArgs e)
	{
		DisplayAlert(Title, "Information about the Task List app", "OK");
	}
}