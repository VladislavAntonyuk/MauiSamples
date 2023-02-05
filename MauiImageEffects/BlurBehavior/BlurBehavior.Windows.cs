namespace MauiImageEffects.BlurBehavior;

using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;

public partial class BlurBehavior : PlatformBehavior<Image, Microsoft.UI.Xaml.Controls.Image>
{
	Microsoft.UI.Xaml.Controls.Image? imageView;
	protected override async void OnAttachedTo(Image bindable, Microsoft.UI.Xaml.Controls.Image platformView)
	{
		imageView = platformView;
		await Task.Delay(300);
		SetRendererEffect(platformView, Radius);
	}

	protected override void OnDetachedFrom(Image bindable, Microsoft.UI.Xaml.Controls.Image platformView)
	{
		SetRendererEffect(platformView, 0);
	}

	void SetRendererEffect(Microsoft.UI.Xaml.Controls.Image imageView, float radius)
	{
		var graphicsEffect = new GaussianBlurEffect()
		{
			Name = "Blur",
			Source = new CompositionEffectSourceParameter("Source"),
			BlurAmount = radius
		};

		var compositor = ElementCompositionPreview.GetElementVisual(imageView).Compositor;
		var blurEffectFactory = compositor.CreateEffectFactory(graphicsEffect);

		var brush = blurEffectFactory.CreateBrush();
		var destinationBrush = compositor.CreateBackdropBrush();
		brush.SetSourceParameter("Source", destinationBrush);

		var blurSprite = compositor.CreateSpriteVisual();
		blurSprite.Brush = brush;
		blurSprite.Size = imageView.ActualSize;
		ElementCompositionPreview.SetElementChildVisual(imageView, blurSprite);
	}
}