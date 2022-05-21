namespace PizzaStore.WebApp.Extensions;

using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
	public static string GetDescription(this Enum enumValue)
	{
		return enumValue.GetType()
						.GetMember(enumValue.ToString())
						.First()
						.GetCustomAttribute<DescriptionAttribute>()
						?.Description ??
			   enumValue.ToString();
	}

	public static T? GetValueFromDescription<T>(this string description) where T : Enum
	{
		foreach (var field in typeof(T).GetFields())
		{
			if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
			{
				if (attribute.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase))
				{
					return (T?)field.GetValue(null);
				}
			}
			else
			{
				if (field.Name.Equals(description, StringComparison.InvariantCultureIgnoreCase))
				{
					return (T?)field.GetValue(null);
				}
			}
		}

		return default;
	}
}