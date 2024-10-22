namespace KanbanBoard;

using System.Globalization;

[AcceptEmptyServiceProvider]
public class BoolToColorConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		var isBeingDragged = (bool?)value;
		var result = (isBeingDragged ?? false) ? Color.FromArgb("#bcacdc") : Color.FromArgb("#FFFFFF");
		return result;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value;
	}
}