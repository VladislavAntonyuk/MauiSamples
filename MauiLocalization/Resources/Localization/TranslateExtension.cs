namespace MauiLocalization.Resources.Localization;

[ContentProperty(nameof(Name))]
public class TranslateExtension : IMarkupExtension<BindingBase>
{
	public string? Name { get; set; }
	public object? BindingContext { get; set; }
	public IValueConverter? Converter { get; set; }
	public object? ConverterParameter { get; set; }

	public BindingBase ProvideValue(IServiceProvider serviceProvider)
	{
		var label = new Label { BindingContext = BindingContext };
		label.SetBinding(Label.TextProperty, new Binding(Name));
		var value = label.GetValue(Label.TextProperty) ?? Name;
		return new Binding
		{
			Mode = BindingMode.OneWay,
			Path = $"[{value}]",
			Source = LocalizationResourceManager.Instance,
			Converter = Converter,
			ConverterParameter = ConverterParameter
		};
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
	{
		return ProvideValue(serviceProvider);
	}
}