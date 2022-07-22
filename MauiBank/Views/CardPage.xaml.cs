namespace MauiBank.Views;

using ViewModels;

public partial class CardPage : ContentPage
{
	public CardPage(CardPageViewModel cardPageViewModel)
	{
		InitializeComponent();
		BindingContext = cardPageViewModel;
	}
}