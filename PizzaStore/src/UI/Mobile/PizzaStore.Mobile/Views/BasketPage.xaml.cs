namespace PizzaStore.Mobile.Views;

using ViewModels;
using Application = Microsoft.Maui.Controls.Application;

public partial class BasketPage : ContentPage
{
	public BasketPage(BasketViewModel basketViewModel)
	{
		InitializeComponent();
		BindingContext = basketViewModel;
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await ((AppShell)Application.Current!.MainPage!).GoToAsync("//MainPage");
	}

	protected override bool OnBackButtonPressed() => false;
}