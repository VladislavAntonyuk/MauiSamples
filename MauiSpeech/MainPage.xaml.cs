namespace MauiSpeech;

using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenAI.Chat;

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
	public partial List<Locale>? Locales { get; set; }

	[ObservableProperty]
	public partial Locale? Locale { get; set; }

	[ObservableProperty]
	public partial string Text { get; set; }

	[ObservableProperty]
	public partial string? ApiKey { get; set; }

	[ObservableProperty]
	public partial string? RecognitionText { get; set; }

	public MainViewModel(ITextToSpeech textToSpeech, ISpeechToText speechToText)
	{
		this.textToSpeech = textToSpeech;
		this.speechToText = speechToText;
		Locales = new();
		Text = @"Glory to Ukraine!
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
		try
		{
			// https://platform.openai.com/docs/models/model-endpoint-compatibility
			var chatClient = new ChatClient("gpt-4o-mini", ApiKey);
			var result = await chatClient.CompleteChatAsync(generalPrompt);
			Process.Start(result.Value.Content.ToString());
		}
		catch (Exception e)
		{
			await Toast.Make(e.Message).Show(CancellationToken.None);
		}
	}
#endif
}