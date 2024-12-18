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

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		await mainViewModel.Initialize();
	}
}

public partial class MainViewModel : ObservableObject
{
	private readonly IConfigCatClient configCatClient;
	private readonly UserContext userContext;

	[ObservableProperty]
	public partial string Title { get; set; }

	[ObservableProperty]
	public partial string Image { get; set; }

	public MainViewModel(IConfigCatClient configCatClient, UserContext userContext)
	{
		this.configCatClient = configCatClient;
		configCatClient.ConfigChanged += async (_, _) =>
		{
			await Initialize();
		};
		this.userContext = userContext;
		Image = "bot.png";
		Title = "Main Page";
	}

	public async Task Initialize()
	{
		Title = await configCatClient.GetValueAsync("beta_gmailusers_mainpagetitle", "Main Page", new User(userContext.Email)
		{
			Email = userContext.Email
		});
		Image = await configCatClient.GetValueAsync("beta", false) ? "botbeta.png" : "bot.png";
	}

	[RelayCommand]
	private async Task Logout()
	{
		userContext.Email = string.Empty;
		var page = Application.Current?.Windows.LastOrDefault()?.Page as AppShell;
		if (page is null)
		{
			ArgumentNullException.ThrowIfNull(page);
		}

		await page.GoToAsync("///LoginPage");
	}
}