namespace MauiBank.Views;

using ViewModels;

public partial class ProfilePage : BasePage
{
	public ProfilePage(ProfilePageViewModel profilePageViewModel)
	{
		InitializeComponent();
		BindingContext = profilePageViewModel;
	}
}