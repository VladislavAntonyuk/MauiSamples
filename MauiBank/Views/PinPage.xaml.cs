namespace MauiBank.Views;

using ViewModels;

public partial class PinPage : ContentPage
{
	public PinPage(PinPageViewModel pinPageViewModel)
	{
		InitializeComponent();
		BindingContext = pinPageViewModel;
	}
}