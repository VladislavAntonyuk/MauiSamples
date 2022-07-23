namespace MauiBank.ViewModels;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

public partial class PinPageViewModel : ObservableObject
{
	[ObservableProperty]
	private string pin = string.Empty;

	[RelayCommand]
	async Task KeyboardButtonClicked(string parameter, CancellationToken cancellationToken)
	{
		Pin += parameter;
		switch (Pin.Length)
		{
			case < 4:
				break;
			case 4:
				await ValidatePin(cancellationToken);
				break;
			case > 4:
				break;
		}
	}

	async Task ValidatePin(CancellationToken cancellationToken)
	{
		const string userPin = "1111";
		if (Pin == userPin)
		{
			await GetMainPage().GoToAsync("//home");
		}
		else
		{
			await Toast.Make("Pin is not valid").Show(cancellationToken);
		}

		Pin = string.Empty;
	}

	[RelayCommand]
	void DeleteButtonClicked()
	{
		if (Pin.Length > 0)
		{
			Pin = Pin[..^1];
		}
	}

	[RelayCommand]
	async Task BiometryAuthClicked(CancellationToken cancellationToken)
	{
#pragma warning disable CA1416
		var request = new AuthenticationRequestConfiguration("Prove you have fingers!", "Because without it you can't have access");
		var result = await CrossFingerprint.Current.AuthenticateAsync(request, cancellationToken);
		if (result.Authenticated)
		{
			await GetMainPage().GoToAsync("//home");
		}
		else
		{
			await Toast.Make("Biometric authentication is not supported").Show(cancellationToken);
		}
#pragma warning restore CA1416
	}

	[RelayCommand]
	async Task ForgotPasswordButtonClicked()
	{
		var result = await GetMainPage().DisplayAlert(
			"Restore password", "This will open browser. Do you want to continue?"
			, "OK",
			"Cancel");
		if (result)
		{
			await Browser.OpenAsync("https://google.com");
		}
	}

	AppShell GetMainPage()
	{
		ArgumentNullException.ThrowIfNull(Application.Current?.MainPage);
		return (AppShell)Application.Current.MainPage;
	}
}