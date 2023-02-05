namespace MauiImageEffects.SaturationBehavior;

using Android.Graphics;
using Android.Widget;

public partial class SaturationBehavior : PlatformBehavior<Image, ImageView>
{
	ImageView? imageView;
	protected override void OnAttachedTo(Image bindable, ImageView platformView)
	{
		imageView = platformView;
		SetRendererEffect(platformView, Saturation);
	}

	protected override void OnDetachedFrom(Image bindable, ImageView platformView)
	{
		SetRendererEffect(platformView, 0);
	}

	void SetRendererEffect(ImageView imageView, float radius)
	{
		if (OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			var renderEffect = radius > 0 ? GetEffect(radius) : null;
			imageView.SetRenderEffect(renderEffect);
		}
		else
		{

		}
	}

	static RenderEffect? GetEffect(float saturation)
	{
		if (OperatingSystem.IsAndroidVersionAtLeast(31))
		{
			var colorMatrix = new ColorMatrix();
			colorMatrix.SetSaturation(saturation);
			return RenderEffect.CreateColorFilterEffect(new ColorMatrixColorFilter(colorMatrix));
		}

		return null;
	}
}