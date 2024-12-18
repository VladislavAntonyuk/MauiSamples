namespace MauiDynamicConfiguration;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConfigCat.Client;

public partial class LoginPage : ContentPage
{
	private readonly LoginViewModel loginViewModel;

	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		this.loginViewModel = loginViewModel;
		BindingContext = this.loginViewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		await loginViewModel.Initialize();
	}
}

public partial class LoginViewModel(IConfigCatClient configCatClient, UserContext userContext) : ObservableObject
{
	[ObservableProperty]
	public partial string? Email { get; set; }

	[ObservableProperty]
	public partial string? Title { get; set; }

	public async Task Initialize()
	{
		Title = await configCatClient.GetValueAsync("beta", false) ? "Beta Login" : "Login";
	}

	[RelayCommand]
	private async Task Login()
	{
		if (string.IsNullOrEmpty(Email))
		{
			return;
		}

		userContext.Email = Email;
		var page = Application.Current?.Windows.LastOrDefault()?.Page as AppShell;
		if (page is null)
		{
			ArgumentNullException.ThrowIfNull(page);
		}

		await page.GoToAsync("///MainPage");
	}
}