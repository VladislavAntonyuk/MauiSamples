namespace MauiBank.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class ProfilePageViewModel : ObservableObject
{
	[RelayCommand]
	Task Back()
	{
		return GetMainPage().GoToAsync("//home/CardPage", true);
	}

	AppShell GetMainPage()
	{
		var page = Application.Current?.Windows.LastOrDefault()?.Page as AppShell;
		if (page is null)
		{
			ArgumentNullException.ThrowIfNull(page);
		}

		return page;
	}
}