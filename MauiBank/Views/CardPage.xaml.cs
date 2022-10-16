namespace MauiBank.Views;

using ViewModels;

public partial class CardPage : BasePage
{
	public CardPage(CardPageViewModel cardPageViewModel)
	{
		InitializeComponent();
		BindingContext = cardPageViewModel;
	}
}