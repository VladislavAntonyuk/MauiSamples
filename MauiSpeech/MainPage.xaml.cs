namespace MauiSpeech;

using System.Globalization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
		var isAuthorized = await speechToText.RequestPermissions();
		if (isAuthorized)
		{
			try
			{
				RecognitionText = await speechToText.Listen(CultureInfo.GetCultureInfo(Locale?.Language ?? "en-us"), new Progress<string>(partialText =>
					{
						RecognitionText += partialText + " ";
					}), cancellationToken);
			}
			catch (Exception ex)
			{
				await Toast.Make(ex.Message).Show(cancellationToken);
			}
		}
		else
		{
			await Toast.Make("Permission denied").Show(cancellationToken);
		}
	}
}