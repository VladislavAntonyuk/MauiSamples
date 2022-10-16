namespace MauiBank.Views;

using CommunityToolkit.Maui.Core.Platform;
using ViewModels;

public partial class PinPage : BasePage
{
	public PinPage(PinPageViewModel pinPageViewModel)
	{
		InitializeComponent();
		BindingContext = pinPageViewModel;
	}
}