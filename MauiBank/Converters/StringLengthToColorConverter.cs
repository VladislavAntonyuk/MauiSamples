namespace MauiBank.Converters;

using System.Globalization;

public class StringLengthToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var parameterInt = int.Parse(parameter.ToString() ?? "0");
		return value is string valueString
			? valueString.Length >= parameterInt ? Colors.Red : Color.FromRgb(30, 30, 30)
			: Colors.Transparent;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}