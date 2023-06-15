namespace MauiBank.Views;

using ViewModels;

public partial class PinPage : BasePage
{
	public PinPage(PinPageViewModel pinPageViewModel)
	{
		InitializeComponent();
		BindingContext = pinPageViewModel;
	}
}