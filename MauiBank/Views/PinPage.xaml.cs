namespace MauiBank.Views;

using System.Globalization;
using System.Runtime.Versioning;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;

public partial class PinPage : ContentPage
{
	public PinPage(PinPageViewModel pinPageViewModel)
	{
		InitializeComponent();
		BindingContext = pinPageViewModel;
	}
}

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
	async Task BiometryAuthClicked()
	{
#pragma warning disable CA1416
		var request = new AuthenticationRequestConfiguration("Prove you have fingers!", "Because without it you can't have access");
		var result = await CrossFingerprint.Current.AuthenticateAsync(request);
		if (result.Authenticated)
		{
			await GetMainPage().GoToAsync("//home");
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

public class StringLengthToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var parameterInt = int.Parse(parameter.ToString() ?? "0");
		return value is string valueString
			? valueString.Length >= parameterInt ? Colors.Red : Color.FromRgb(30,30,30)
			: Colors.Transparent;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

