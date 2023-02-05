namespace PizzaStore.Mobile.Views;

using CommunityToolkit.Maui.Views;
using ViewModels;
using Application = Microsoft.Maui.Controls.Application;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
		BindingContext = mainViewModel;
	}

	private async void OrderButton_Clicked(object sender, EventArgs e)
	{
		await ((AppShell)Application.Current!.MainPage!).GoToAsync("//BasketPage");
	}

	private void Help_Clicked(object sender, EventArgs e)
	{
		PizzasCollectionView.ScrollTo(0);
		var simplePopup = new PopupTutorial();
		this.ShowPopup(simplePopup);
	}
}