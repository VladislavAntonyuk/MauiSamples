namespace MauiImageEffects.ChainBehavior;

using Tizen.NUI;
using Tizen.NUI.BaseComponents;

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

	}
}