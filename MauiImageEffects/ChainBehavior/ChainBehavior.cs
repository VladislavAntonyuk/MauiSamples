namespace MauiImageEffects.ChainBehavior;

public partial class ChainBehavior
{
	public static readonly BindableProperty EffectsProperty = BindableProperty.Create(nameof(Effects), typeof(string), typeof(ChainBehavior), string.Empty, propertyChanged: OnChanged);

	public string? Effects
	{
		get => (string)GetValue(EffectsProperty);
		set => SetValue(EffectsProperty, value);
	}

	static void OnChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var behavior = (ChainBehavior)bindable;
		if (behavior.imageView is null)
		{
			return;
		}

		behavior.SetRendererEffect(behavior.imageView, Convert.ToString(newValue));
	}
}