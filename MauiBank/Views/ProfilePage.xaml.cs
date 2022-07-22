namespace MauiBank.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel profilePageViewModel)
	{
		InitializeComponent();
		BindingContext = profilePageViewModel;
	}
}

public partial class ProfilePageViewModel : ObservableObject
{
	[RelayCommand]
	Task Back()
	{
		return GetMainPage().GoToAsync("//home/CardPage", true);
	}

	AppShell GetMainPage()
	{
		ArgumentNullException.ThrowIfNull(Application.Current?.MainPage);
		return (AppShell)Application.Current.MainPage;
	}
}