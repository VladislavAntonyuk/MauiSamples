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

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		loginViewModel.Initialize();
	}
}

public partial class LoginViewModel : ObservableObject
{
	private readonly UserContext userContext;
	private readonly IConfigCatClient configCatClient;

	[ObservableProperty]
	string? email;

	[ObservableProperty]
	string title = "Login";

	public LoginViewModel(IConfigCatClient configCatClient, UserContext userContext)
	{
		this.userContext = userContext;
		this.configCatClient = configCatClient;
	}

	public void Initialize()
	{
		Title = this.configCatClient.GetValue("beta", false) ? "Beta Login" : "Login";
	}

	[RelayCommand]
	private async Task Login()
	{
		if (string.IsNullOrEmpty(Email))
		{
			return;
		}

		userContext.Email = Email;
		await ((AppShell)Application.Current!.MainPage!).GoToAsync("///MainPage");
	}
}