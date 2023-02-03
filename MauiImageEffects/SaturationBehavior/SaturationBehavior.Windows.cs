namespace MauiImageEffects.SaturationBehavior;

using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;

public partial class SaturationBehavior : PlatformBehavior<Image, Microsoft.UI.Xaml.Controls.Image>
{
	Microsoft.UI.Xaml.Controls.Image? imageView;
	protected override async void OnAttachedTo(Image bindable, Microsoft.UI.Xaml.Controls.Image platformView)
	{
		imageView = platformView;
		await Task.Delay(300);
		SetRendererEffect(platformView, Saturation);
	}

	protected override void OnDetachedFrom(Image bindable, Microsoft.UI.Xaml.Controls.Image platformView)
	{
		SetRendererEffect(platformView, 0);
	}

	void SetRendererEffect(Microsoft.UI.Xaml.Controls.Image imageView, float saturation)
	{
		var graphicsEffect = new SaturationEffect()
		{
			Name = "Saturation",
			Source = new CompositionEffectSourceParameter("Source"),
			Saturation = saturation
		};

		var compositor = ElementCompositionPreview.GetElementVisual(imageView).Compositor;
		var effectFactory = compositor.CreateEffectFactory(graphicsEffect);

		var brush = effectFactory.CreateBrush();
		var destinationBrush = compositor.CreateBackdropBrush();
		brush.SetSourceParameter("Source", destinationBrush);

		var sprite = compositor.CreateSpriteVisual();
		sprite.Brush = brush;
		sprite.Size = imageView.ActualSize;
		ElementCompositionPreview.SetElementChildVisual(imageView, sprite);
	}
}