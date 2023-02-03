namespace MauiImageEffects.SaturationBehavior;

using System.Globalization;

internal class SaturationConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (double.TryParse(value?.ToString(), out var doubleValue))
		{
			return Math.Clamp(doubleValue / 100, 0, 1);
		}

		return 0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}