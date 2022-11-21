namespace MauiSpeech;

using System.Globalization;
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

	[ObservableProperty]
	private List<Locale>? locales;

	[ObservableProperty]
	private Locale? locale;

	[ObservableProperty]
	private string text;

	public MainViewModel(ITextToSpeech textToSpeech)
	{
		this.textToSpeech = textToSpeech;
		Locales = new();
		text = @"Slava Uycriayini!
Heroyim Slava!
Slava Naaseee!
Smert vorogaam!
Uycriayina - ponad uysae!
Uycriayina - ponad uysae!
Uycriayina - ponad uysae!";
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
		await TextToSpeech.Default.SpeakAsync(Text, new SpeechOptions()
		{
			Locale = Locale,
			Pitch = 2,
			Volume = 1
		}, cancellationToken);
	}
}

public class PickerDisplayConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Locale locale)
		{
			return $"{locale.Language} {locale.Name}";
		}

		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}