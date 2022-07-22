namespace MauiBank.Converters;

using System.Globalization;

public class BoolToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return System.Convert.ToBoolean(value) ? Colors.LightGreen : Colors.LightGray;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}