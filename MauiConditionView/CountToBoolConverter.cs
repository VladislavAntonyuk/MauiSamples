namespace MauiConditionView;

using System.Globalization;
using CommunityToolkit.Maui.Converters;

public class CountToBoolConverter : BaseConverterOneWay<int, bool>
{
	public override bool ConvertFrom(int value, CultureInfo? culture)
	{
		return value < 5;
	}

	public override bool DefaultConvertReturnValue { get; set; }
}