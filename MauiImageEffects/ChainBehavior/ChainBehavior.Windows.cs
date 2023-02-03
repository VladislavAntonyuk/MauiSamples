namespace MauiImageEffects.ChainBehavior;

using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using Windows.Graphics.Effects;

public partial class ChainBehavior : PlatformBehavior<Image, Microsoft.UI.Xaml.Controls.Image>
{
	Microsoft.UI.Xaml.Controls.Image? imageView;
	protected override async void OnAttachedTo(Image bindable, Microsoft.UI.Xaml.Controls.Image platformView)
	{
		imageView = platformView;
		await Task.Delay(300);
		SetRendererEffect(platformView, Effects);
	}

	protected override void OnDetachedFrom(Image bindable, Microsoft.UI.Xaml.Controls.Image platformView)
	{
		SetRendererEffect(platformView, null);
	}

	void SetRendererEffect(Microsoft.UI.Xaml.Controls.Image imageView, string? effects)
	{
		var compositor = ElementCompositionPreview.GetElementVisual(imageView).Compositor;
		var effectFactory = compositor.CreateEffectFactory(GetEffect(effects));

		var brush = effectFactory.CreateBrush();
		var destinationBrush = compositor.CreateBackdropBrush();
		brush.SetSourceParameter("Source", destinationBrush);

		var sprite = compositor.CreateSpriteVisual();
		sprite.Brush = brush;
		sprite.Size = imageView.ActualSize;
		ElementCompositionPreview.SetElementChildVisual(imageView, sprite);
	}

	static IGraphicsEffect? GetEffect(string? effects)
	{
		var effectNames = effects?.Split(',', StringSplitOptions.RemoveEmptyEntries);
		return effectNames?.Length switch
		{
			null => null,
			1 => CreateEffectByName(effectNames[0]),
			_ => CreateChainEffect(effectNames)
		};
	}

	static IGraphicsEffect? CreateEffectByName(string effectName, IGraphicsEffectSource? source = null)
	{
		return effectName switch
		{
			"blur" => new GaussianBlurEffect()
			{
				Name = "Blur",
				Source = source ?? new CompositionEffectSourceParameter("Source"),
				BlurAmount = 5
			},
			"saturation" => new SaturationEffect()
			{
				Name = "Saturation",
				Source = source ?? new CompositionEffectSourceParameter("Source"),
				Saturation = 0.05f
			},
			_ => null
		};
	}

	static IGraphicsEffect? CreateChainEffect(string[] effects)
	{
		var effect = CreateEffectByName(effects[0]);
		for (var index = 1; index < effects.Length; index++)
		{
			var effectName = effects[index];
			effect = CreateEffectByName(effectName, effect);
		}

		return effect;
	}
}