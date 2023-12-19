namespace MauiBells.Calendar;

using System.ComponentModel;
using System.Globalization;

public class DateTimeOffsetStringConverter : TypeConverter
{
	public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		if (value is string valueString)
		{
			return DateTimeOffset.Parse(valueString);
		}

		return DateTimeOffset.MinValue;
	}
}