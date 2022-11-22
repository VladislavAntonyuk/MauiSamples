namespace MauiSpeech;

using System.Globalization;

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