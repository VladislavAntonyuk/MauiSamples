namespace MauiDynamicConfiguration;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConfigCat.Client;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel mainViewModel;

	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
		this.mainViewModel = mainViewModel;
		BindingContext = mainViewModel;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		mainViewModel.Initialize();
	}
}

public partial class MainViewModel : ObservableObject
{
	private readonly IConfigCatClient configCatClient;
	private readonly UserContext userContext;

	[ObservableProperty]
	string title = "Main Page";


	[ObservableProperty]
	string image = "bot.png";

	public MainViewModel(IConfigCatClient configCatClient, UserContext userContext)
	{
		this.configCatClient = configCatClient;
		configCatClient.ConfigChanged += (_, _) =>
		{
			Initialize();
		};
		this.userContext = userContext;
	}

	public void Initialize()
	{
		Title = this.configCatClient.GetValue("beta_gmailusers_mainpagetitle", "Main Page", new User(userContext.Email)
		{
			Email = userContext.Email
		});
		Image = this.configCatClient.GetValue("beta", false) ? "botbeta.png" : "bot.png";
	}

	[RelayCommand]
	private async Task Logout()
	{
		userContext.Email = string.Empty;
		await ((AppShell)Application.Current!.MainPage!).GoToAsync("///LoginPage");
	}
}