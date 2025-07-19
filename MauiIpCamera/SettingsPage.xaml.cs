namespace MauiIpCamera;

using ViewModels;

public partial class SettingsPage
{
	public SettingsPage(CameraViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}