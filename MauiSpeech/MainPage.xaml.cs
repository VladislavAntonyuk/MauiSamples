namespace MauiSpeech;

using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
		BindingContext = mainViewModel;
	}
}

public partial class MainViewModel : ObservableObject
{
	private readonly ITextToSpeech textToSpeech;
	private readonly ISpeechToText speechToText;

	[ObservableProperty]
	private List<Locale>? locales;

	[ObservableProperty]
	private Locale? locale;

	[ObservableProperty]
	private string text;

	[ObservableProperty]
	private string? apiKey;

	[ObservableProperty]
	private string? recognitionText;

	public MainViewModel(ITextToSpeech textToSpeech, ISpeechToText speechToText)
	{
		this.textToSpeech = textToSpeech;
		this.speechToText = speechToText;
		Locales = new();
		text = @"Glory to Ukraine!
Glory to heroes!
Glory to the Nation!
Death to enemies!";
		SetLocalesCommand.Execute(null);
	}

	[RelayCommand]
	async Task SetLocales()
	{
		Locales = (await textToSpeech.GetLocalesAsync()).ToList();
		Locale = Locales.FirstOrDefault();
	}

	[RelayCommand]
	async Task Play(CancellationToken cancellationToken)
	{
		await textToSpeech.SpeakAsync(Text, new SpeechOptions()
		{
			Locale = Locale,
			Pitch = 2,
			Volume = 1
		}, cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	async Task Listen(CancellationToken cancellationToken)
	{
		RecognitionText = string.Empty;
		var isAuthorized = await speechToText.RequestPermissions(CancellationToken.None);
		if (isAuthorized)
		{
			var recognitionResult = await speechToText.ListenAsync(CultureInfo.GetCultureInfo(Locale?.Language ?? "en-us"), new Progress<string>(async partialText =>
			{
				RecognitionText += partialText + " ";
#if WINDOWS
				await ProcessText(partialText);
#else
				await Task.CompletedTask;
#endif
			}), CancellationToken.None);

			if (recognitionResult.IsSuccessful)
			{
				RecognitionText = recognitionResult.Text;
			}
			else
			{
				await Toast.Make(recognitionResult.Exception.Message).Show(CancellationToken.None);
			}
		}
		else
		{
			await Toast.Make("Permission denied").Show(CancellationToken.None);
		}
	}
#if WINDOWS
	async Task ProcessText(string prompt)
	{
		var generalPrompt = $"You should return an executable name of the program. example prompt: Execute Word. Expected output WinWord. Do not return extension. So my request: {prompt}";
		var api = new OpenAIAPI(ApiKey);
		// https://platform.openai.com/docs/models/model-endpoint-compatibility
		var result = await api.Completions.CreateCompletionAsync(new CompletionRequest(generalPrompt)
		{
			Model = Model.DavinciText
		});
		try
		{
			Process.Start(result.Completions[0].Text);
		}
		catch (Exception e)
		{
			await Toast.Make(e.Message).Show(CancellationToken.None);
		}
	}
#endif
}