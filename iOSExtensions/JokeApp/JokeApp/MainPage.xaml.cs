namespace JokeApp;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
	}
}

public partial class MainPageViewModel : ObservableObject
{
	private readonly IJokeApi jokeApi;

	[ObservableProperty]
	private Joke? joke;

	[ObservableProperty]
	private string? jokeType;

	[ObservableProperty]
	private string[] jokeTypes = Array.Empty<string>();

	public MainPageViewModel(IJokeApi jokeApi)
	{
		this.jokeApi = jokeApi;
		GetJokeTypesCommand.Execute(null);
	}

	[RelayCommand]
	private async Task GetJokeTypes()
	{
		JokeTypes = await jokeApi.GetTypes();
		JokeType = JokeTypes.FirstOrDefault();
		await GetJokeByType();
	}

	[RelayCommand]
	private async Task GetJokeByType()
	{
		if (string.IsNullOrEmpty(JokeType))
		{
			Joke = await jokeApi.GetRandomJoke();
			return;
		}

		var jokes = await jokeApi.GetJokeByType(JokeType);
		Joke = jokes.FirstOrDefault();
	}
}

public class Joke
{
	public int Id { get; set; }
	public string? Type { get; set; }
	public string? Setup { get; set; }
	public string? Punchline { get; set; }
}

public interface IJokeApi
{
	[Get("/")]
	Task<Joke> GetRandomJoke();

	[Get("/type")]
	Task<string[]> GetTypes();

	[Get("/type/{type}/1")]
	Task<Joke[]> GetJokeByType(string type);
}