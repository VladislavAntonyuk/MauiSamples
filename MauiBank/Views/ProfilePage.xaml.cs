namespace MauiBank.Views;

using CommunityToolkit.Maui.Core.Platform;
using ViewModels;

public partial class ProfilePage : BasePage
{
	public ProfilePage(ProfilePageViewModel profilePageViewModel)
	{
		InitializeComponent();
		BindingContext = profilePageViewModel;
	}
}