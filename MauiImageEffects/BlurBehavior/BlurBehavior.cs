namespace MauiImageEffects.BlurBehavior;

public partial class BlurBehavior
{
	public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(float), typeof(BlurBehavior), 10f, propertyChanged: OnRadiusChanged);

	public float Radius
	{
		get => (float)GetValue(RadiusProperty);
		set => SetValue(RadiusProperty, value);
	}

	static void OnRadiusChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var behavior = (BlurBehavior)bindable;
		if (behavior.imageView is null)
		{
			return;
		}

		behavior.SetRendererEffect(behavior.imageView, Convert.ToSingle(newValue));
	}
}