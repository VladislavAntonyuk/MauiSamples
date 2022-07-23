namespace MauiImageEffects.ChainBehavior;

using Android.Graphics;
using Android.Widget;

public partial class ChainBehavior : PlatformBehavior<Image, ImageView>
{
	ImageView? imageView;
	protected override void OnAttachedTo(Image bindable, ImageView platformView)
	{
		imageView = platformView;
		SetRendererEffect(platformView, Effects);
	}

	protected override void OnDetachedFrom(Image bindable, ImageView platformView)
	{
		SetRendererEffect(platformView, null);
	}

	void SetRendererEffect(ImageView imageView, string? effects)
	{
		if (OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			var effect = GetEffect(effects);
			imageView.SetRenderEffect(effect);
		}
		else
		{

		}
	}

	static RenderEffect? GetEffect(string? effects)
	{
		if (!OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			return null;
		}

		var effectNames = effects?.Split(',', StringSplitOptions.RemoveEmptyEntries);
		return effectNames?.Length switch
		{
			null => null,
			1 => CreateEffectByName(effectNames[0]),
			_ => CreateChainEffect(effectNames)
		};
	}

	static RenderEffect? CreateEffectByName(string effectName)
	{
		if (!OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			return null;
		}

		switch (effectName)
		{
			case "blur":
				return RenderEffect.CreateBlurEffect(5, 5, Shader.TileMode.Decal!);
			case "saturation":
				var colorMatrix = new ColorMatrix();
				colorMatrix.SetSaturation(0.05f);
				return RenderEffect.CreateColorFilterEffect(new ColorMatrixColorFilter(colorMatrix));
			default:
				return null;
		}
	}

	static RenderEffect? CreateChainEffect(string[] effects)
	{
		if (!OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			return null;
		}

		var outerEffect = CreateEffectByName(effects[0]);
		if (effects.Length == 1)
		{
			return outerEffect;
		}

		var innerEffectNames = effects[1..];

		return RenderEffect.CreateChainEffect(
			outerEffect!,
			CreateChainEffect(innerEffectNames)!);
	}
}