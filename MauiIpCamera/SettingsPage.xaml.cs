namespace MauiIpCamera;

using ViewModels;

public partial class SettingsPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}