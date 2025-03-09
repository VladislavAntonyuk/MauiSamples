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

	[RelayCommand]
	async Task ListenCancel()
	{
		await speechToText.StopListenAsync();
		speechToText.RecognitionResultUpdated -= SpeechToText_RecognitionResultUpdated;
		speechToText.RecognitionResultCompleted -= SpeechToText_RecognitionResultCompleted;
	}

	[RelayCommand]
	async Task Listen()
	{
		RecognitionText = string.Empty;
		var isAuthorized = await speechToText.RequestPermissions(CancellationToken.None);
		if (isAuthorized)
		{
			speechToText.RecognitionResultUpdated += SpeechToText_RecognitionResultUpdated;
			speechToText.RecognitionResultCompleted += SpeechToText_RecognitionResultCompleted;

			await speechToText.StartListenAsync(new SpeechToTextOptions()
			{
				Culture = CultureInfo.GetCultureInfo(Locale?.Language ?? "en-us")
			});
		}
		else
		{
			await Toast.Make("Permission denied").Show(CancellationToken.None);
		}
	}

	private async void SpeechToText_RecognitionResultCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs e)
	{
		if (e.RecognitionResult.IsSuccessful)
		{
			RecognitionText = e.RecognitionResult.Text;
		}
		else
		{
			await Toast.Make(e.RecognitionResult.Exception.Message).Show();
		}
	}

	private async void SpeechToText_RecognitionResultUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
	{
		RecognitionText += e.RecognitionResult + " ";
#if WINDOWS
		await ProcessText(e.RecognitionResult);
#else
		await Task.CompletedTask;
#endif
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