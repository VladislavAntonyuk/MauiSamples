namespace MauiBank;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		if (Application.Current != null)
		{
			Application.Current.UserAppTheme = AppTheme.Dark;

		}
	}
}