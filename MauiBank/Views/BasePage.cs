namespace MauiBank.Views;

using CommunityToolkit.Maui.Core.Platform;

public abstract class BasePage : ContentPage
{
	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
		{
			StatusBar.SetColor(Colors.Black);
		}
	}
}