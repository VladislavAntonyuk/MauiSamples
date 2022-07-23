namespace MauiImageEffects.SaturationBehavior;

public partial class SaturationBehavior
{
	public static readonly BindableProperty SaturationProperty = BindableProperty.Create(nameof(Saturation), typeof(float), typeof(SaturationBehavior), 10f, propertyChanged: OnChanged);

	public float Saturation
	{
		get => (float)GetValue(SaturationProperty);
		set => SetValue(SaturationProperty, value);
	}

	static void OnChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var behavior = (SaturationBehavior)bindable;
		if (behavior.imageView is null)
		{
			return;
		}

		behavior.SetRendererEffect(behavior.imageView, Convert.ToSingle(newValue));
	}
}